using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Laboris.Migrations
{
    public partial class editBasketItems2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "basketItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "basketItems");
        }
    }
}
