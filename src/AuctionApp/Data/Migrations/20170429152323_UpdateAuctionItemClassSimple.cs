using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuctionApp.Data.Migrations
{
    public partial class UpdateAuctionItemClassSimple : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_AuctionItems_ItemId",
                table: "Bids");

            migrationBuilder.DropIndex(
                name: "IX_Bids_ItemId",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Bids");

            migrationBuilder.AddColumn<int>(
                name: "AuctionItemId",
                table: "Bids",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "AuctionItems",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Bids_AuctionItemId",
                table: "Bids",
                column: "AuctionItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_AuctionItems_AuctionItemId",
                table: "Bids",
                column: "AuctionItemId",
                principalTable: "AuctionItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_AuctionItems_AuctionItemId",
                table: "Bids");

            migrationBuilder.DropIndex(
                name: "IX_Bids_AuctionItemId",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "AuctionItemId",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "AuctionItems");

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Bids",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bids_ItemId",
                table: "Bids",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_AuctionItems_ItemId",
                table: "Bids",
                column: "ItemId",
                principalTable: "AuctionItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
