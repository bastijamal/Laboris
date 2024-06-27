using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Laboris.Migrations
{
    public partial class OrderTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_basketItems_AspNetUsers_UserId",
                table: "basketItems");

            migrationBuilder.DropForeignKey(
                name: "FK_basketItems_Products_ProductId",
                table: "basketItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_basketItems",
                table: "basketItems");

            migrationBuilder.RenameTable(
                name: "basketItems",
                newName: "BasketItems");

            migrationBuilder.RenameIndex(
                name: "IX_basketItems_UserId",
                table: "BasketItems",
                newName: "IX_BasketItems_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_basketItems_ProductId",
                table: "BasketItems",
                newName: "IX_BasketItems_ProductId");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "BasketItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BasketItems",
                table: "BasketItems",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    TotalPrice = table.Column<double>(type: "float", nullable: false),
                    PurchaseAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Postcode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasketItems_OrderId",
                table: "BasketItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItems_AspNetUsers_UserId",
                table: "BasketItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItems_Orders_OrderId",
                table: "BasketItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItems_Products_ProductId",
                table: "BasketItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItems_AspNetUsers_UserId",
                table: "BasketItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BasketItems_Orders_OrderId",
                table: "BasketItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BasketItems_Products_ProductId",
                table: "BasketItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BasketItems",
                table: "BasketItems");

            migrationBuilder.DropIndex(
                name: "IX_BasketItems_OrderId",
                table: "BasketItems");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "BasketItems");

            migrationBuilder.RenameTable(
                name: "BasketItems",
                newName: "basketItems");

            migrationBuilder.RenameIndex(
                name: "IX_BasketItems_UserId",
                table: "basketItems",
                newName: "IX_basketItems_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BasketItems_ProductId",
                table: "basketItems",
                newName: "IX_basketItems_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_basketItems",
                table: "basketItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_basketItems_AspNetUsers_UserId",
                table: "basketItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_basketItems_Products_ProductId",
                table: "basketItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
