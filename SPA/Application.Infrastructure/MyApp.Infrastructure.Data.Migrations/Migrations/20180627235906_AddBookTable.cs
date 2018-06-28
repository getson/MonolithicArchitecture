using Microsoft.EntityFrameworkCore.Migrations;

namespace MyApp.Infrastructure.Data.Migrations.Migrations
{
    public partial class AddBookTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Isbn",
                table: "Product",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Publisher",
                table: "Product",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Isbn",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Publisher",
                table: "Product");
        }
    }
}
