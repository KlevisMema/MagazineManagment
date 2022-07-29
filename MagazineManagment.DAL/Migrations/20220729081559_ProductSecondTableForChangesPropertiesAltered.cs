using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagazineManagment.DAL.Migrations
{
    public partial class ProductSecondTableForChangesPropertiesAltered : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductRecordsChangeds_Categories_ProductCategoryId",
                table: "ProductRecordsChangeds");

            migrationBuilder.DropIndex(
                name: "IX_ProductRecordsChangeds_ProductCategoryId",
                table: "ProductRecordsChangeds");

            migrationBuilder.DropColumn(
                name: "ProductCategoryId",
                table: "ProductRecordsChangeds");

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyType",
                table: "ProductRecordsChangeds",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductCategory",
                table: "ProductRecordsChangeds",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductCategory",
                table: "ProductRecordsChangeds");

            migrationBuilder.AlterColumn<int>(
                name: "CurrencyType",
                table: "ProductRecordsChangeds",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductCategoryId",
                table: "ProductRecordsChangeds",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecordsChangeds_ProductCategoryId",
                table: "ProductRecordsChangeds",
                column: "ProductCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRecordsChangeds_Categories_ProductCategoryId",
                table: "ProductRecordsChangeds",
                column: "ProductCategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
