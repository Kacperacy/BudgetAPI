﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetAPI.Migrations
{
    /// <inheritdoc />
    public partial class CategoryNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_Categories_CategoryId",
                schema: "identity",
                table: "Budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Categories_CategoryId",
                schema: "identity",
                table: "Expenses");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                schema: "identity",
                table: "Expenses",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                schema: "identity",
                table: "Budgets",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_Categories_CategoryId",
                schema: "identity",
                table: "Budgets",
                column: "CategoryId",
                principalSchema: "identity",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Categories_CategoryId",
                schema: "identity",
                table: "Expenses",
                column: "CategoryId",
                principalSchema: "identity",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_Categories_CategoryId",
                schema: "identity",
                table: "Budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Categories_CategoryId",
                schema: "identity",
                table: "Expenses");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                schema: "identity",
                table: "Expenses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                schema: "identity",
                table: "Budgets",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_Categories_CategoryId",
                schema: "identity",
                table: "Budgets",
                column: "CategoryId",
                principalSchema: "identity",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Categories_CategoryId",
                schema: "identity",
                table: "Expenses",
                column: "CategoryId",
                principalSchema: "identity",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
