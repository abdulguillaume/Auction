using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using AuctionApp.Models;
using System.Collections.Generic;
using System.IO;

namespace AuctionApp.Data
{
    public class SampleData
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            var db = serviceProvider.GetService<ApplicationDbContext>();
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            // Ensure db
            db.Database.EnsureCreated();

            // Ensure Admin (IsAdmin)
            var admin = await userManager.FindByNameAsync("admin@arimex.ht");
            if (admin == null)
            {
                // create user
                admin = new ApplicationUser
                {
                    UserName = "admin@arimex.ht",
                    Email = "admin@arimex.ht"
                };
                await userManager.CreateAsync(admin, "Secret123!");

                // add claims
                await userManager.AddClaimAsync(admin, new Claim("IsAdmin", "true"));
            }

            // Ensure Abdul (not IsAdmin)
            // normal user
            
            var abdul = await userManager.FindByNameAsync("abdul@local.ht");
            if (abdul == null)
            {
                 //create user
                abdul = new ApplicationUser
                {
                    UserName = "abdul@local.ht",
                    Email = "abdul@local.ht"
                };
                await userManager.CreateAsync(abdul, "Secret123!");
            }

            if (!db.AuctionItems.Any())
            {
                var img1 = File.ReadAllBytes("wwwroot/images/data/sandals.jpg");
                var img2 = File.ReadAllBytes("wwwroot/images/data/mug01.jpg");
                var img3 = File.ReadAllBytes("wwwroot/images/data/mug02.jpg");

                var auctionItem1 = new AuctionItem
                {
                    Name = "Reaction KC/Sandals",
                    Description = "Pair of Sandals/Make Reaction Kenneth Cole /Material: Leather",
                    MinimumBid = 5m,
                    NumberOfBids = 2,
                    CreatedBy = "admin@arimex.ht",
                    CreatedDate = DateTime.Now,
                    Images = new List<ItemImage>() {

                        new ItemImage
                        {
                            Pic = img1
                        }
                    }
                };

                var auctionItem2 = new AuctionItem
                {
                    Name = "Coffee Mug",
                    Description = "Bill Gates coffee mug in 12th grade",
                    MinimumBid = 20m,
                    NumberOfBids = 6,
                    CreatedBy = "abdul@local.ht",
                    CreatedDate = DateTime.Now,
                    Images = new List<ItemImage>() {

                        new ItemImage
                        {
                            Pic = img2
                        },

                        new ItemImage
                        {
                            Pic = img3
                        }
                    }
                };

                db.AuctionItems.Add(
                    auctionItem1
                    );

                db.AuctionItems.Add(
                    auctionItem2
                    );

                db.SaveChanges();
            }
        }

    }
}
