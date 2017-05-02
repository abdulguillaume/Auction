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

            // Ensure Mike (not IsAdmin)
            /*
            var mike = await userManager.FindByNameAsync("Mike@CoderCamps.com");
            if (mike == null)
            {
                // create user
                mike = new ApplicationUser
                {
                    UserName = "Mike@CoderCamps.com",
                    Email = "Mike@CoderCamps.com"
                };
                await userManager.CreateAsync(mike, "Secret123!");
            }*/

            if (!db.AuctionItems.Any())
            {
                var img = File.ReadAllBytes("wwwroot/images/data/sandals.jpg");

                var auctionItem = new AuctionItem
                {
                    Name = "Reaction KC/Sandals",
                    Description = "Pair of Sandals/Make Reaction Kenneth Cole /Material: Leather",
                    MinimumBid = 5m,
                    NumberOfBids = 2,
                    CreatedDate = DateTime.Now,
                    Images = new List<ItemImage>() {

                        new ItemImage
                        {
                            Pic = img
                        }
                    }
                };

                db.AuctionItems.Add(
                    auctionItem
                    );

                db.SaveChanges();
            }
        }

    }
}
