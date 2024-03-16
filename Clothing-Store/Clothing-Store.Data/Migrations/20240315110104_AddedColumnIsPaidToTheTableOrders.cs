using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clothing_Store.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedColumnIsPaidToTheTableOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Orders");
        }
    }
}
