using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clothing_Store.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactoringTablesOrdersCustomersAndCreatedNewOneOrderProducts : Migration
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

            migrationBuilder.CreateTable(
                name: "OrderProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderProducts_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_OrderId",
                table: "OrderProducts",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderProducts");

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
