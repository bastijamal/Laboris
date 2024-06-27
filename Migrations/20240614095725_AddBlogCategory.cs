using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Laboris.Migrations
{
    public partial class AddBlogCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_BlogCategory_CategoryId",
                table: "Blogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogCategory",
                table: "BlogCategory");

            migrationBuilder.RenameTable(
                name: "BlogCategory",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Categories_CategoryId",
                table: "Blogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "BlogCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogCategory",
                table: "BlogCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_BlogCategory_CategoryId",
                table: "Blogs",
                column: "CategoryId",
                principalTable: "BlogCategory",
                principalColumn: "Id");
        }
    }
}
