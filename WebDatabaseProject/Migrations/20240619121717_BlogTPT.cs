using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDatabaseProject.Migrations
{
    public partial class BlogTPT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentBlogId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blogs_Blogs_ParentBlogId",
                        column: x => x.ParentBlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BlogConfigs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogId = table.Column<long>(type: "bigint", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ValueTypeId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogConfigs_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopBlogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ShopUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopBlogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopBlogs_Blogs_Id",
                        column: x => x.Id,
                        principalTable: "Blogs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TechBlogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    GithubUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechBlogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechBlogs_Blogs_Id",
                        column: x => x.Id,
                        principalTable: "Blogs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogConfigs_BlogId_Key",
                table: "BlogConfigs",
                columns: new[] { "BlogId", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_ParentBlogId",
                table: "Blogs",
                column: "ParentBlogId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogConfigs");

            migrationBuilder.DropTable(
                name: "ShopBlogs");

            migrationBuilder.DropTable(
                name: "TechBlogs");

            migrationBuilder.DropTable(
                name: "Blogs");
        }
    }
}
