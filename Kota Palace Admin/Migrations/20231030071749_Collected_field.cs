using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kota_Palace_Admin.Migrations
{
    /// <inheritdoc />
    public partial class Collected_field : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Collected",
                table: "Order",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Collected",
                table: "Order");
        }
    }
}
