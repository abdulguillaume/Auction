using AuctionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.ViewModels
{
    public class AuctionItemWithBids
    {
        public AuctionItem Item { get; set; }
        public List<Bid> Bids { get; set; }

        public decimal getMax() {
            if (Bids == null || Bids.Count == 0)
                return Item.MinimumBid;

            return Bids.Max(b => b.BidAmount);
        }
    }
}
