using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        public decimal GetMax()
        {
            //let's keep both conditions separate
            //if (Bids == null || Bids.Count == 0)
              //  return MinimumBid;

            return Bids.Max(b => b.BidAmount);
        }

        public List<StringImage> ImgToBase64()
        {
            List<StringImage> imgs = new List<StringImage>();

            foreach (var img in Images)
            {
                imgs.Add(
                    new StringImage
                    {
                        Id = img.Id,
                        Pic = Format(img.Pic)
                    }
                    );
            };

            return imgs;
        }

        private string Format(byte[] pic)
        {
            return string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(pic));
        }
    }

}
