using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDatabaseProject.Migrations
{
    public partial class ChangeKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogAttributeValues_BlogAttributes_BlogAttributeId",
                table: "BlogAttributeValues");

            migrationBuilder.DropIndex(
                name: "IX_BlogAttributeValues_BlogAttributeId",
                table: "BlogAttributeValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogAttributes",
                table: "BlogAttributes");

            migrationBuilder.DropColumn(
                name: "BlogAttributeId",
                table: "BlogAttributeValues");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BlogAttributes");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BlogAttributeValues",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "BlogAttributes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogAttributes",
                table: "BlogAttributes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_BlogAttributeValues_Name",
                table: "BlogAttributeValues",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogAttributeValues_BlogAttributes_Name",
                table: "BlogAttributeValues",
                column: "Name",
                principalTable: "BlogAttributes",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogAttributeValues_BlogAttributes_Name",
                table: "BlogAttributeValues");

            migrationBuilder.DropIndex(
                name: "IX_BlogAttributeValues_Name",
                table: "BlogAttributeValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogAttributes",
                table: "BlogAttributes");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BlogAttributeValues");

            migrationBuilder.AddColumn<long>(
                name: "BlogAttributeId",
                table: "BlogAttributeValues",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "BlogAttributes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "BlogAttributes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogAttributes",
                table: "BlogAttributes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BlogAttributeValues_BlogAttributeId",
                table: "BlogAttributeValues",
                column: "BlogAttributeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogAttributeValues_BlogAttributes_BlogAttributeId",
                table: "BlogAttributeValues",
                column: "BlogAttributeId",
                principalTable: "BlogAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
