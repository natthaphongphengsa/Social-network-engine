using Microsoft.EntityFrameworkCore.Migrations;

namespace TDDInlämningsuppgift.Migrations
{
    public partial class Fixname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Datum",
                table: "Posters",
                newName: "Date");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Posters",
                newName: "Datum");
        }
    }
}
