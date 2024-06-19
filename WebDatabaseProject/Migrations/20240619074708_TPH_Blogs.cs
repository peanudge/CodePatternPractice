using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDatabaseProject.Migrations
{
    public partial class TPH_Blogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GithubUrl",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ParentBlogId",
                table: "Blogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShopUrl",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_ParentBlogId",
                table: "Blogs",
                column: "ParentBlogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Blogs_ParentBlogId",
                table: "Blogs",
                column: "ParentBlogId",
                principalTable: "Blogs",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Blogs_ParentBlogId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_ParentBlogId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "GithubUrl",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "ParentBlogId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "ShopUrl",
                table: "Blogs");
        }
    }
}
