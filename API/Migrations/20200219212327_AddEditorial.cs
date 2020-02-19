using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class AddEditorial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EditorialId",
                table: "Books",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Editorial",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Editorial", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_EditorialId",
                table: "Books",
                column: "EditorialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Editorial_EditorialId",
                table: "Books",
                column: "EditorialId",
                principalTable: "Editorial",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Editorial_EditorialId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "Editorial");

            migrationBuilder.DropIndex(
                name: "IX_Books_EditorialId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "EditorialId",
                table: "Books");
        }
    }
}
