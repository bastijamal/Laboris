using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Laboris.Migrations
{
    public partial class editBlogCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Categories_CategoryId",
                table: "Blogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "BlogCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogCategories",
                table: "BlogCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_BlogCategories_CategoryId",
                table: "Blogs",
                column: "CategoryId",
                principalTable: "BlogCategories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_BlogCategories_CategoryId",
                table: "Blogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogCategories",
                table: "BlogCategories");

            migrationBuilder.RenameTable(
                name: "BlogCategories",
                newName: "Categories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Categories_CategoryId",
                table: "Blogs",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
