using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAppBackend.Migrations
{
    /// <inheritdoc />
    public partial class AppChat1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserChat_ChatId",
                table: "UserChat",
                column: "ChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserChat_AspNetUsers_UserId",
                table: "UserChat",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserChat_Chats_ChatId",
                table: "UserChat",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserChat_AspNetUsers_UserId",
                table: "UserChat");

            migrationBuilder.DropForeignKey(
                name: "FK_UserChat_Chats_ChatId",
                table: "UserChat");

            migrationBuilder.DropIndex(
                name: "IX_UserChat_ChatId",
                table: "UserChat");
        }
    }
}
