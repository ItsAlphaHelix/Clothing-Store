using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clothing_Store.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedKeySizeNameToProductsBagTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductBags",
                table: "ProductBags");

            migrationBuilder.AlterColumn<string>(
                name: "SizeName",
                table: "ProductBags",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductBags",
                table: "ProductBags",
                columns: new[] { "ProductId", "BagId", "SizeName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductBags",
                table: "ProductBags");

            migrationBuilder.AlterColumn<string>(
                name: "SizeName",
                table: "ProductBags",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductBags",
                table: "ProductBags",
                columns: new[] { "ProductId", "BagId" });
        }
    }
}
