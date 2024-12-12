using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealTimeChatAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImageToChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Chats",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Chats");
        }
    }
}
