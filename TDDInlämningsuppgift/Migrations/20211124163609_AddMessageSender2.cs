using Microsoft.EntityFrameworkCore.Migrations;

namespace TDDInlämningsuppgift.Migrations
{
    public partial class AddMessageSender2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Users_SendById",
                table: "Chats");

            migrationBuilder.RenameColumn(
                name: "SendById",
                table: "Chats",
                newName: "SendToId");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_SendById",
                table: "Chats",
                newName: "IX_Chats_SendToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Users_SendToId",
                table: "Chats",
                column: "SendToId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Users_SendToId",
                table: "Chats");

            migrationBuilder.RenameColumn(
                name: "SendToId",
                table: "Chats",
                newName: "SendById");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_SendToId",
                table: "Chats",
                newName: "IX_Chats_SendById");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Users_SendById",
                table: "Chats",
                column: "SendById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
