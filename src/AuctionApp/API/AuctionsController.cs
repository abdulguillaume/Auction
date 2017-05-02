using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//using System.Data.Entity;
using AuctionApp.Data;
using AuctionApp.Models;
using Microsoft.EntityFrameworkCore;
using AuctionApp.ViewModels;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AuctionApp.API
{
    [Route("api/[controller]")]
    public class AuctionsController : Controller
    {

        private ApplicationDbContext _db;

        public AuctionsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<AuctionItemWithByteToBase64> Get()
        {
            /*return _db.AuctionItems
                .Include(p => p.Images).ToList();*/

            //var items = _db.AuctionItems
            //    .Include(b => b.Bids)
            //    .Include(p=> p.Images)
            //    .Include(p => new List<StringImage> { p.ImgToBase64() })
            //    .ToList();

            var items = from i in _db.AuctionItems
                        .Include(p => p.Images)
                        .Include(b => b.Bids)
                        select new AuctionItemWithByteToBase64
                        {
                            Id = i.Id,
                            Name = i.Name,
                            Description = i.Description,
                            MinimumBid= i.MinimumBid,
                            NumberOfBids = i.NumberOfBids,
                            CreatedDate = i.CreatedDate,
                            Images = i.ImgToBase64(),
                            Bids = i.Bids
                        };

            return items;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var auction = _db.AuctionItems
                //.Include(p => p.Images)
                .Include(b => b.Bids)
                .FirstOrDefault(a => a.Id == id);
                
            if (auction == null)
            {
                return BadRequest(this.ModelState);
            }

            return Ok(auction);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]AuctionItem auction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(this.ModelState);
            }

            if (auction.Id == 0)
            {
                //add a new auction
                auction.CreatedDate = DateTime.Now;
                _db.AuctionItems.Add(auction);
                _db.SaveChanges();
            }
            else
            {
                //update auction - add new bid
                var oldAuction = _db.AuctionItems
                                    .Include(b=>b.Bids)  //no need to load all at this time
                                    .FirstOrDefault(a => auction.Id == a.Id);

                //var aBid = auction.Bids[0];//.FirstOrDefault(b => b.Id == 0);

                if (oldAuction != null)
                {
                    var newBid = new Bid() {
                        BidDate = DateTime.Now,
                        BidAmount = auction.Bids[0].BidAmount,
                        Customer = auction.Bids[0].Customer
                    };

                   if(oldAuction.Bids == null)
                        oldAuction.Bids = new List<Bid>();

                    if (oldAuction.Bids.Count==oldAuction.NumberOfBids)
                        return BadRequest("Auction is closed. Ceiling number has been reached!");

                    if (oldAuction.Bids.Count>0 && newBid.BidAmount <= oldAuction.GetMax())
                        return BadRequest("You should place a higher bid!");

                    if (oldAuction.Bids.Count == 0 && newBid.BidAmount < oldAuction.MinimumBid)
                        return BadRequest(string.Format("Minimum Bid is {0}!",oldAuction.MinimumBid));

                    oldAuction.Bids.Add(newBid);
                    auction = oldAuction;
                    _db.SaveChanges();
                }
            }

            return Ok(auction);
        }

        // PUT api/values/5
        /*[HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }*/

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
