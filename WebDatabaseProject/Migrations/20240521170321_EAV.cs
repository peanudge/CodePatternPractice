using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDatabaseProject.Migrations
{
    public partial class EAV : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogAttributes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogAttributes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogAttributeValues",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogId = table.Column<long>(type: "bigint", nullable: false),
                    BlogAttributeId = table.Column<long>(type: "bigint", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogAttributeValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogAttributeValues_BlogAttributes_BlogAttributeId",
                        column: x => x.BlogAttributeId,
                        principalTable: "BlogAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogAttributeValues_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogAttributeValues_BlogAttributeId",
                table: "BlogAttributeValues",
                column: "BlogAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogAttributeValues_BlogId",
                table: "BlogAttributeValues",
                column: "BlogId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogAttributeValues");

            migrationBuilder.DropTable(
                name: "BlogAttributes");
        }
    }
}
