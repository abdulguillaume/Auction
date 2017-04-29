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
        public IEnumerable<AuctionItemWithBids> Get()
        {
            /*return _db.AuctionItems
                .Include(p => p.Images).ToList();*/
            
            var items = _db.AuctionItems
                .Include(p => p.Images)
                .Select(i => new AuctionItemWithBids
                {
                    Item = i,
                    Bids = _db.Bids.Where(b => b.Item == i).ToList()

                }).ToList();

            return items;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var auction = _db.AuctionItems
                .Include(p => p.Images)
                .Where(a => a.Id == id)
                .Select(i => new AuctionItemWithBids
                {
                    Item = i,
                    Bids = _db.Bids.Where(b => b.Item == i).ToList()

                }).FirstOrDefault();
                
            if (auction == null)
            {
                return BadRequest(this.ModelState);
            }

            return Ok(auction);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]Bid bid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(this.ModelState);
            }

            /*  if (bid.Id != 0)
              {
                  //throw exception
                  ;
              }*/

            bid.BidDate = DateTime.Now;

            _db.Bids.Add(bid);
            _db.SaveChanges();
            
            return Ok(bid);
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
