using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clothing_Store.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedColumnsIsDeletedDeletedOnModifiedOnAndCreatedOnInTableProductFavorites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Favorites");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ProductFavorites",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "ProductFavorites",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProductFavorites",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "ProductFavorites",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ProductFavorites");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "ProductFavorites");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProductFavorites");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "ProductFavorites");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Favorites",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
