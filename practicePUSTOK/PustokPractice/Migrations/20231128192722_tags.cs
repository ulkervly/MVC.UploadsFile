using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PustokPractice.Migrations
{
    public partial class tags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Books_Bookid",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_Bookid",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Bookid",
                table: "Tags");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Bookid",
                table: "Tags",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Bookid",
                table: "Tags",
                column: "Bookid");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Books_Bookid",
                table: "Tags",
                column: "Bookid",
                principalTable: "Books",
                principalColumn: "id");
        }
    }
}
