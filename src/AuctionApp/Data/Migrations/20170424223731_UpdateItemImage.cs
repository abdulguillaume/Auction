using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuctionApp.Data.Migrations
{
    public partial class UpdateItemImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemImages_AuctionItems_ItemId",
                table: "ItemImages");

            migrationBuilder.DropIndex(
                name: "IX_ItemImages_ItemId",
                table: "ItemImages");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "ItemImages");

            migrationBuilder.AddColumn<int>(
                name: "AuctionItemId",
                table: "ItemImages",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemImages_AuctionItemId",
                table: "ItemImages",
                column: "AuctionItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemImages_AuctionItems_AuctionItemId",
                table: "ItemImages",
                column: "AuctionItemId",
                principalTable: "AuctionItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemImages_AuctionItems_AuctionItemId",
                table: "ItemImages");

            migrationBuilder.DropIndex(
                name: "IX_ItemImages_AuctionItemId",
                table: "ItemImages");

            migrationBuilder.DropColumn(
                name: "AuctionItemId",
                table: "ItemImages");

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "ItemImages",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemImages_ItemId",
                table: "ItemImages",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemImages_AuctionItems_ItemId",
                table: "ItemImages",
                column: "ItemId",
                principalTable: "AuctionItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
