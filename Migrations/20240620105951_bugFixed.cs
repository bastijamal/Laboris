using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Laboris.Migrations
{
    public partial class bugFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductTag_Products_ProductsId",
                table: "ProductTag");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductTag_Tags_TagId",
                table: "ProductTag");

            migrationBuilder.DropIndex(
                name: "IX_ProductTag_ProductsId",
                table: "ProductTag");

            migrationBuilder.DropColumn(
                name: "ProductsId",
                table: "ProductTag");

            migrationBuilder.DropColumn(
                name: "TagName",
                table: "ProductTag");

            migrationBuilder.AlterColumn<int>(
                name: "TagId",
                table: "ProductTag",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ProductTag",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductTag_ProductId",
                table: "ProductTag",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTag_Products_ProductId",
                table: "ProductTag",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTag_Tags_TagId",
                table: "ProductTag",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductTag_Products_ProductId",
                table: "ProductTag");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductTag_Tags_TagId",
                table: "ProductTag");

            migrationBuilder.DropIndex(
                name: "IX_ProductTag_ProductId",
                table: "ProductTag");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductTag");

            migrationBuilder.AlterColumn<int>(
                name: "TagId",
                table: "ProductTag",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ProductsId",
                table: "ProductTag",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TagName",
                table: "ProductTag",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTag_ProductsId",
                table: "ProductTag",
                column: "ProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTag_Products_ProductsId",
                table: "ProductTag",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTag_Tags_TagId",
                table: "ProductTag",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id");
        }
    }
}
