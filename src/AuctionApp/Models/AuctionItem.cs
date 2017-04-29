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

        //public string ImageUrl { get; set; }
        public List<ItemImage> Images { get; set; }

    }
}
