using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionApp.Models;

namespace AuctionApp.ViewModels
{
    public class AuctionItemWithByteToBase64
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal MinimumBid { get; set; }

        public int NumberOfBids { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<StringImage> Images { get; set; }

        public List<Bid> Bids { get; set; }
    }
}
