using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Persistence.Migrations
{
    public partial class uniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "Projects",
                newName: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Name",
                table: "Projects",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Projects_Name",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Projects",
                newName: "name");
        }
    }
}
