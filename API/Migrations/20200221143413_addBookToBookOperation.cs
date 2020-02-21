using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class addBookToBookOperation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BookId",
                table: "BookOperations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookOperations_BookId",
                table: "BookOperations",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookOperations_Books_BookId",
                table: "BookOperations",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookOperations_Books_BookId",
                table: "BookOperations");

            migrationBuilder.DropIndex(
                name: "IX_BookOperations_BookId",
                table: "BookOperations");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "BookOperations");
        }
    }
}
