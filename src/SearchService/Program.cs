using System.Net;
using MongoDB.Driver;
using MongoDB.Entities;
using Polly;
using Polly.Extensions.Http;
using SearchService.Data;
using SearchService.Models;
using SearchService.Services;
using MassTransit;
using SearchService.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // autoMapper
builder.Services.AddHttpClient<AuctionSvcHttpClient>().AddPolicyHandler(GetPolicy()); //service injected
builder.Services.AddMassTransit(x =>
{
  x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();

  x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));

  x.UsingRabbitMq((context, cfg) =>
  {
    cfg.ReceiveEndpoint("search-auction-created", e =>
    {
      e.UseMessageRetry(r => r.Interval(5, 5));

      e.ConfigureConsumer<AuctionCreatedConsumer>(context);
    });

    cfg.Host(builder.Configuration["RabbitMq:Host"], "/", h =>
    {
      h.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
      h.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
    });

    cfg.ConfigureEndpoints(context);
  });
}); // RabbitMQ


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthorization();
app.MapControllers();
app.Lifetime.ApplicationStarted.Register(async () =>
{
  try
  {
    await DbInitializer.InitDb(app);
  }
  catch (Exception ex)
  {
    Console.WriteLine(ex);
  }
});

app.Run();

static IAsyncPolicy<HttpResponseMessage> GetPolicy() => HttpPolicyExtensions.HandleTransientHttpError().
                                                        OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                                                        .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3)); // Polly

