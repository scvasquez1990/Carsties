using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.Data;
using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Repositories
{
    public class SQLAuctionRepository : IAuctionRepository
    {
        private readonly AuctionDbContext _dbContext;

        public SQLAuctionRepository(AuctionDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Auction>> GetAllAsync()
        {
            var auctions = await _dbContext.Auctions.Include(x => x.Item).OrderBy(x => x.Item.Make).ToListAsync();

            return auctions;
        }
        public async Task<Auction> GetByIdAsync(Guid id)
        {
            var auction = await _dbContext.Auctions.FirstOrDefaultAsync(x => x.Id == id);
            return auction;
        }
        public async Task<Auction> CreateAsync(Auction auction)
        {
            await _dbContext.Auctions.AddAsync(auction);
            await _dbContext.SaveChangesAsync();
            return auction;
        }
        /*public async Task<Auction> UpdateAsync(Guid id, Auction auction)
        {
            var domainAuction = await _dbContext.Auctions.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == id);
            if (domainAuction != null)
            {
                domainAuction.Item.Make = auction.Item.Make ?? domainAuction.Item.Make;
                domainAuction.Item.Model = auction.Item.Model ?? domainAuction.Item.Model;
                domainAuction.Item.Color = auction.Item.Color ?? domainAuction.Item.Color;
                //domainAuction.Item.Mileage = auction.Item.Mileage ?? domainAuction.Item.Mileage;
                //domainAuction.Item.Make = auction.Item.Year ?? domainAuction.Item.Year;
            }
        }*/
        public async Task<Auction> DeleteByIdAsync(Guid id)
        {
            var auction = await _dbContext.Auctions.FirstOrDefaultAsync(x => x.Id == id);
            if (auction != null)
            {
                _dbContext.Auctions.Remove(auction);
            }

            return auction;
        }
    }
}