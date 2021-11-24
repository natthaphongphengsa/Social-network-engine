using Microsoft.EntityFrameworkCore.Migrations;

namespace TDDInlämningsuppgift.Migrations
{
    public partial class AddMessageSender3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SendFromId",
                table: "Chats",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendFromId",
                table: "Chats");
        }
    }
}
