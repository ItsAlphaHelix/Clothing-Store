using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clothing_Store.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumnNameToUserFullNameINTableProductReviewsAlsoRemoveEmailAddressColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "ProductsReviews");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ProductsReviews",
                newName: "UserFullName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserFullName",
                table: "ProductsReviews",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "ProductsReviews",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
