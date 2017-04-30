using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.Models
{
    public class AuctionItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal MinimumBid { get; set; }

        public int NumberOfBids { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<ItemImage> Images { get; set; }

        public List<Bid> Bids { get; set; }

        public decimal getMax()
        {
            if (Bids == null || Bids.Count == 0)
                return MinimumBid;

            return Bids.Max(b => b.BidAmount);
        }

    }
}
