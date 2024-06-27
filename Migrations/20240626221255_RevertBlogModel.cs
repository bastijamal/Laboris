using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Laboris.Migrations
{
    public partial class RevertBlogModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BlogId",
                table: "Blogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Search",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_BlogId",
                table: "Blogs",
                column: "BlogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Blogs_BlogId",
                table: "Blogs",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Blogs_BlogId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_BlogId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "BlogId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Search",
                table: "Blogs");
        }
    }
}
