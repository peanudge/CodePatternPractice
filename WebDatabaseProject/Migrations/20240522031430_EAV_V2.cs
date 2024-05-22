using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDatabaseProject.Migrations
{
    public partial class EAV_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "GithubUrl",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "LinkedInUrl",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BlogAttributeValues");

            migrationBuilder.AddColumn<long>(
                name: "AttributeId",
                table: "BlogAttributeValues",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "BlogAttributes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_BlogAttributes_Name",
                table: "BlogAttributes",
                column: "Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogAttributes",
                table: "BlogAttributes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BlogAttributeValues_AttributeId",
                table: "BlogAttributeValues",
                column: "AttributeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogAttributeValues_BlogAttributes_AttributeId",
                table: "BlogAttributeValues",
                column: "AttributeId",
                principalTable: "BlogAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogAttributeValues_BlogAttributes_AttributeId",
                table: "BlogAttributeValues");

            migrationBuilder.DropIndex(
                name: "IX_BlogAttributeValues_AttributeId",
                table: "BlogAttributeValues");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_BlogAttributes_Name",
                table: "BlogAttributes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogAttributes",
                table: "BlogAttributes");

            migrationBuilder.DropColumn(
                name: "AttributeId",
                table: "BlogAttributeValues");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BlogAttributes");

            migrationBuilder.AddColumn<string>(
                name: "GithubUrl",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedInUrl",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BlogAttributeValues",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

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
    }
}
