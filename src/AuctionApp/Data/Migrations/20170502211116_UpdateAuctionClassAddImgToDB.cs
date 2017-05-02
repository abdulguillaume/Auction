using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuctionApp.Data.Migrations
{
    public partial class UpdateAuctionClassAddImgToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ItemImages");

            migrationBuilder.AddColumn<byte[]>(
                name: "Pic",
                table: "ItemImages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pic",
                table: "ItemImages");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "ItemImages",
                nullable: true);
        }
    }
}
