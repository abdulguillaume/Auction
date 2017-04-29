using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionApp.Models
{
    public class Bid
    {
        public int Id { get; set; }
        public AuctionItem Item { get; set; }
        public decimal BidAmount { get; set; }
        public string Customer { get; set; }//the authenticated user who placed the bid
        public DateTime BidDate { get; set; }
    }
}
