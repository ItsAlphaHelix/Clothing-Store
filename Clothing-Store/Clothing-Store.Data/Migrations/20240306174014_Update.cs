using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clothing_Store.Data.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBags_Orders_OrderId",
                table: "ProductBags");

            migrationBuilder.DropIndex(
                name: "IX_ProductBags_OrderId",
                table: "ProductBags");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "ProductBags");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Bags",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bags_OrderId",
                table: "Bags",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bags_Orders_OrderId",
                table: "Bags",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bags_Orders_OrderId",
                table: "Bags");

            migrationBuilder.DropIndex(
                name: "IX_Bags_OrderId",
                table: "Bags");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Bags");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "ProductBags",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductBags_OrderId",
                table: "ProductBags",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBags_Orders_OrderId",
                table: "ProductBags",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
