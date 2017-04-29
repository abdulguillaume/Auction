using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuctionApp.Data.Migrations
{
    public partial class RenamingModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bid_AuctionItem_ItemId",
                table: "Bid");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemImage_AuctionItem_ItemId",
                table: "ItemImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemImage",
                table: "ItemImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bid",
                table: "Bid");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuctionItem",
                table: "AuctionItem");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemImages",
                table: "ItemImage",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bids",
                table: "Bid",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuctionItems",
                table: "AuctionItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_AuctionItems_ItemId",
                table: "Bid",
                column: "ItemId",
                principalTable: "AuctionItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemImages_AuctionItems_ItemId",
                table: "ItemImage",
                column: "ItemId",
                principalTable: "AuctionItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.RenameIndex(
                name: "IX_ItemImage_ItemId",
                table: "ItemImage",
                newName: "IX_ItemImages_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Bid_ItemId",
                table: "Bid",
                newName: "IX_Bids_ItemId");

            migrationBuilder.RenameTable(
                name: "ItemImage",
                newName: "ItemImages");

            migrationBuilder.RenameTable(
                name: "Bid",
                newName: "Bids");

            migrationBuilder.RenameTable(
                name: "AuctionItem",
                newName: "AuctionItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_AuctionItems_ItemId",
                table: "Bids");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemImages_AuctionItems_ItemId",
                table: "ItemImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemImages",
                table: "ItemImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bids",
                table: "Bids");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuctionItems",
                table: "AuctionItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemImage",
                table: "ItemImages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bid",
                table: "Bids",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuctionItem",
                table: "AuctionItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bid_AuctionItem_ItemId",
                table: "Bids",
                column: "ItemId",
                principalTable: "AuctionItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemImage_AuctionItem_ItemId",
                table: "ItemImages",
                column: "ItemId",
                principalTable: "AuctionItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.RenameIndex(
                name: "IX_ItemImages_ItemId",
                table: "ItemImages",
                newName: "IX_ItemImage_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Bids_ItemId",
                table: "Bids",
                newName: "IX_Bid_ItemId");

            migrationBuilder.RenameTable(
                name: "ItemImages",
                newName: "ItemImage");

            migrationBuilder.RenameTable(
                name: "Bids",
                newName: "Bid");

            migrationBuilder.RenameTable(
                name: "AuctionItems",
                newName: "AuctionItem");
        }
    }
}
