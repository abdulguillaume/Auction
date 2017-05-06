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
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AuctionApp.API
{
    [Route("api/[controller]")]
    [Consumes("application/json", "application/json-patch+json", "multipart/form-data")]
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
                .Include(p => p.Images)
                .Include(b => b.Bids)
                .FirstOrDefault(a => a.Id == id);
                
            if (auction == null)
            {
                return BadRequest(this.ModelState);
            }


            var auctionToBase64 = new AuctionItemWithByteToBase64
            {
                Id = auction.Id,
                Name = auction.Name,
                Description = auction.Description,
                MinimumBid = auction.MinimumBid,
                NumberOfBids = auction.NumberOfBids,
                CreatedDate = auction.CreatedDate,
                Images = auction.ImgToBase64(),
                Bids = auction.Bids
            };

            return Ok(auctionToBase64);
        }

        private void GetAuctionInFormValues(AuctionItem auction)
        {
            //like mention in controller.ts fields[x] to retrieve form data for auctionToCreate
            auction.Name =  Request.Form["fields[name]"];
            auction.Description =  Request.Form["fields[description]"];
            auction.MinimumBid = decimal.Parse(Request.Form["fields[minimumBid]"]);
            auction.NumberOfBids =  int.Parse(Request.Form["fields[numberOfBids]"]);
        }


        // POST api/values
        [HttpPost]
        [Route("/api/upload")]
        [Authorize]
        public async Task<IActionResult> /*IActionResult*/ PostFile()//[FromBody]AuctionItem auction)
        {
            var files = Request.Form.Files;

            if (files.Count > 3)
                return BadRequest("Can't load more than three (3) images!");

            AuctionItem auction = new AuctionItem();

            GetAuctionInFormValues(auction);

            if (string.IsNullOrEmpty(auction.Name) || string.IsNullOrEmpty(auction.Description) || auction.MinimumBid == 0 || auction.NumberOfBids == 0)
                return BadRequest("All fields are required!");

            auction.CreatedDate = DateTime.Now;

            List<ItemImage> imgs = new List<ItemImage>();

            foreach (var file in files)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (var rs = file.OpenReadStream()) {

                        rs.CopyTo(ms);

                        ItemImage img = new ItemImage
                        {
                            Pic = ms.ToArray()
                        };

                        imgs.Add(img);

                    }
                        
                }
                    
            }

            if (imgs.Count > 0)
                auction.Images = imgs;

            _db.AuctionItems.Add(auction);
            _db.SaveChanges();

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
                    //_db.SaveChanges();
                }
            }

            _db.SaveChanges();

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
