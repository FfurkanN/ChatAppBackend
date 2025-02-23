using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAppBackend.Migrations
{
    /// <inheritdoc />
    public partial class AppMessageFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File_Url",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "Messages",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileUrl",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "FileUrl",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "File_Url",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
