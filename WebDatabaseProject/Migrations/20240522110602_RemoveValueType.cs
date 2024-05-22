using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDatabaseProject.Migrations
{
    public partial class RemoveValueType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogConfigs_ValueTypes_ValueTypeId",
                table: "BlogConfigs");

            migrationBuilder.DropTable(
                name: "ValueTypes");

            migrationBuilder.DropIndex(
                name: "IX_BlogConfigs_ValueTypeId",
                table: "BlogConfigs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ValueTypes",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValueTypes", x => x.TypeId);
                });

            migrationBuilder.InsertData(
                table: "ValueTypes",
                columns: new[] { "TypeId", "Name" },
                values: new object[,]
                {
                    { 5, "True/False" },
                    { 4, "DateTime" },
                    { 3, "Real Number" },
                    { 2, "Number" },
                    { 1, "String" },
                    { 6, "User" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogConfigs_ValueTypeId",
                table: "BlogConfigs",
                column: "ValueTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogConfigs_ValueTypes_ValueTypeId",
                table: "BlogConfigs",
                column: "ValueTypeId",
                principalTable: "ValueTypes",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
