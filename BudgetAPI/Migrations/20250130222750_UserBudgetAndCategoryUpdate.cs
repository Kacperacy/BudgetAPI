using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetAPI.Migrations
{
    /// <inheritdoc />
    public partial class UserBudgetAndCategoryUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_Categories_CategoryId",
                schema: "identity",
                table: "Budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_UserId",
                schema: "identity",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_UserId",
                schema: "identity",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_CategoryId",
                schema: "identity",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "identity",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "identity",
                table: "Budgets");

            migrationBuilder.AddColumn<Guid>(
                name: "BudgetId",
                schema: "identity",
                table: "Categories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "identity",
                table: "Categories",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                schema: "identity",
                table: "Categories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UserBudgets",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    BudgetId = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBudgets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBudgets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBudgets_Budgets_BudgetId",
                        column: x => x.BudgetId,
                        principalSchema: "identity",
                        principalTable: "Budgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_BudgetId",
                schema: "identity",
                table: "Categories",
                column: "BudgetId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CreatedById",
                schema: "identity",
                table: "Categories",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserBudgets_BudgetId",
                schema: "identity",
                table: "UserBudgets",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBudgets_UserId_BudgetId",
                schema: "identity",
                table: "UserBudgets",
                columns: new[] { "UserId", "BudgetId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_CreatedById",
                schema: "identity",
                table: "Categories",
                column: "CreatedById",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Budgets_BudgetId",
                schema: "identity",
                table: "Categories",
                column: "BudgetId",
                principalSchema: "identity",
                principalTable: "Budgets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_CreatedById",
                schema: "identity",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Budgets_BudgetId",
                schema: "identity",
                table: "Categories");

            migrationBuilder.DropTable(
                name: "UserBudgets",
                schema: "identity");

            migrationBuilder.DropIndex(
                name: "IX_Categories_BudgetId",
                schema: "identity",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CreatedById",
                schema: "identity",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "BudgetId",
                schema: "identity",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "identity",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                schema: "identity",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "identity",
                table: "Categories",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                schema: "identity",
                table: "Budgets",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UserId",
                schema: "identity",
                table: "Categories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_CategoryId",
                schema: "identity",
                table: "Budgets",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_Categories_CategoryId",
                schema: "identity",
                table: "Budgets",
                column: "CategoryId",
                principalSchema: "identity",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_UserId",
                schema: "identity",
                table: "Categories",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
