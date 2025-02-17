using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAppBackend.Migrations
{
    /// <inheritdoc />
    public partial class chats1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Chats_AppChatId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_AppChatId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "AppChatId",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "Messages",
                table: "Chats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Messages",
                table: "Chats");

            migrationBuilder.AddColumn<Guid>(
                name: "AppChatId",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_AppChatId",
                table: "Messages",
                column: "AppChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Chats_AppChatId",
                table: "Messages",
                column: "AppChatId",
                principalTable: "Chats",
                principalColumn: "Id");
        }
    }
}
