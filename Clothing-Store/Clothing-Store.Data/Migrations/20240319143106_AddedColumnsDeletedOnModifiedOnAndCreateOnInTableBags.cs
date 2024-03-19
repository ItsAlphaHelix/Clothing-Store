using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clothing_Store.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedColumnsDeletedOnModifiedOnAndCreateOnInTableBags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Bags",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Bags",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Bags",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Bags",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Bags");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Bags");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Bags");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Bags");
        }
    }
}
