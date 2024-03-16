using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clothing_Store.Data.Migrations
{
    /// <inheritdoc />
    public partial class StripeChargeIdIsAddedToTableOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StripeChargeId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripeChargeId",
                table: "Orders");
        }
    }
}
