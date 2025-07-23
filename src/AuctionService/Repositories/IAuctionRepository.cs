using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.DTOs;
using AuctionService.Entities;

namespace AuctionService.Repositories
{
    public interface IAuctionRepository
    {
        Task<List<Auction>> GetAllAsync();
        Task<Auction> GetByIdAsync(Guid id);
        Task<Auction> CreateAsync(Auction auction);
        // Task<Auction> UpdateAsync(Guid id, Auction auction);
        Task<Auction> DeleteByIdAsync(Guid id);
    }
}