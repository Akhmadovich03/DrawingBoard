using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawingBoard.Migrations
{
    /// <inheritdoc />
    public partial class AddDrawingDataToBoard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "DrawingData",
                table: "Boards",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DrawingData",
                table: "Boards");
        }
    }
}
