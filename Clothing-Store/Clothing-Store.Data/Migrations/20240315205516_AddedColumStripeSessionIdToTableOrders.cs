using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clothing_Store.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedColumStripeSessionIdToTableOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StripeSessionId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripeSessionId",
                table: "Orders");
        }
    }
}
