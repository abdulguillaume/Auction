using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuctionApp.Data.Migrations
{
    public partial class UpdateAuctionItemClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "BidAmount",
                table: "Bid",
                nullable: false);

            migrationBuilder.AlterColumn<decimal>(
                name: "MinimumBid",
                table: "AuctionItem",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BidAmount",
                table: "Bid",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "MinimumBid",
                table: "AuctionItem",
                nullable: false);
        }
    }
}
