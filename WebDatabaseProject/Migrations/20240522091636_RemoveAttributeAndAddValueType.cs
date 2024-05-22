using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDatabaseProject.Migrations
{
    public partial class RemoveAttributeAndAddValueType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogAttributeValues_BlogAttributes_AttributeId",
                table: "BlogAttributeValues");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogAttributeValues_Blogs_BlogId",
                table: "BlogAttributeValues");

            migrationBuilder.DropTable(
                name: "BlogAttributes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogAttributeValues",
                table: "BlogAttributeValues");

            migrationBuilder.DropIndex(
                name: "IX_BlogAttributeValues_AttributeId",
                table: "BlogAttributeValues");

            migrationBuilder.DropIndex(
                name: "IX_BlogAttributeValues_BlogId",
                table: "BlogAttributeValues");

            migrationBuilder.DropColumn(
                name: "AttributeId",
                table: "BlogAttributeValues");

            migrationBuilder.RenameTable(
                name: "BlogAttributeValues",
                newName: "BlogConfigs");

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "BlogConfigs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ValueTypeId",
                table: "BlogConfigs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogConfigs",
                table: "BlogConfigs",
                column: "Id");

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
                name: "IX_BlogConfigs_BlogId_Key",
                table: "BlogConfigs",
                columns: new[] { "BlogId", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogConfigs_ValueTypeId",
                table: "BlogConfigs",
                column: "ValueTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogConfigs_Blogs_BlogId",
                table: "BlogConfigs",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogConfigs_ValueTypes_ValueTypeId",
                table: "BlogConfigs",
                column: "ValueTypeId",
                principalTable: "ValueTypes",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogConfigs_Blogs_BlogId",
                table: "BlogConfigs");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogConfigs_ValueTypes_ValueTypeId",
                table: "BlogConfigs");

            migrationBuilder.DropTable(
                name: "ValueTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogConfigs",
                table: "BlogConfigs");

            migrationBuilder.DropIndex(
                name: "IX_BlogConfigs_BlogId_Key",
                table: "BlogConfigs");

            migrationBuilder.DropIndex(
                name: "IX_BlogConfigs_ValueTypeId",
                table: "BlogConfigs");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "BlogConfigs");

            migrationBuilder.DropColumn(
                name: "ValueTypeId",
                table: "BlogConfigs");

            migrationBuilder.RenameTable(
                name: "BlogConfigs",
                newName: "BlogAttributeValues");

            migrationBuilder.AddColumn<long>(
                name: "AttributeId",
                table: "BlogAttributeValues",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogAttributeValues",
                table: "BlogAttributeValues",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BlogAttributes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogAttributes", x => x.Id);
                    table.UniqueConstraint("AK_BlogAttributes_Name", x => x.Name);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogAttributeValues_AttributeId",
                table: "BlogAttributeValues",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogAttributeValues_BlogId",
                table: "BlogAttributeValues",
                column: "BlogId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogAttributeValues_BlogAttributes_AttributeId",
                table: "BlogAttributeValues",
                column: "AttributeId",
                principalTable: "BlogAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogAttributeValues_Blogs_BlogId",
                table: "BlogAttributeValues",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
