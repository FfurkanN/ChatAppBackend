using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAppBackend.Migrations
{
    /// <inheritdoc />
    public partial class AppChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "unreadMessageCount",
                table: "Chats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "unreadMessageCount",
                table: "Chats",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
