using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDatabaseProject.Migrations
{
    public partial class AddTypeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Blogs");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "BlogAttributes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "BlogAttributes");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
