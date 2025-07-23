using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionService.Entities
{
    public class Auction
    {
        public Guid Id { get; set; }

        public int ReservePrice { get; set; } = 0;

        // Username from the authenticated user (e.g., from JWT claims)
        public string Seller { get; set; }

        // Nullable: username of the winner
        public string? Winner { get; set; }

        public int? SoldAmount { get; set; }

        public int? CurrentHighBid { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public DateTime AuctionEnd { get; set; }

        public Status Status { get; set; } = Status.Live;

        // Navigation Property
        public Item Item { get; set; }
    }
}