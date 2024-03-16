using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clothing_Store.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceColumntStripeSessionIdWithStripePaymentStatusInTableOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StripeSessionId",
                table: "Orders",
                newName: "StripePaymentStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StripePaymentStatus",
                table: "Orders",
                newName: "StripeSessionId");
        }
    }
}
