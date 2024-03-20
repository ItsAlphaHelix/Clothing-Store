using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clothing_Store.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedColumnsIsDeletedDeletedOnModifiedOnAndCreatedOnInTableProductsBag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Bags");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ProductBags",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "ProductBags",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProductBags",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "ProductBags",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ProductBags");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "ProductBags");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProductBags");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "ProductBags");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Bags",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
