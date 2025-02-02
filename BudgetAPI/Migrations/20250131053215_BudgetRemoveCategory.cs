using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetAPI.Migrations
{
    /// <inheritdoc />
    public partial class BudgetRemoveCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_BudgetId",
                schema: "identity",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "identity",
                table: "Categories");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_BudgetId",
                schema: "identity",
                table: "Categories",
                column: "BudgetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_BudgetId",
                schema: "identity",
                table: "Categories");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "identity",
                table: "Categories",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Categories_BudgetId",
                schema: "identity",
                table: "Categories",
                column: "BudgetId",
                unique: true);
        }
    }
}
