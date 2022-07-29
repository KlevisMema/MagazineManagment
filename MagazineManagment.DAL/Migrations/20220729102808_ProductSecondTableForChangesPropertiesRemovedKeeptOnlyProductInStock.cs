using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagazineManagment.DAL.Migrations
{
    public partial class ProductSecondTableForChangesPropertiesRemovedKeeptOnlyProductInStock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyType",
                table: "ProductRecordsChangeds");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "ProductRecordsChangeds");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ProductRecordsChangeds");

            migrationBuilder.DropColumn(
                name: "ProductCategory",
                table: "ProductRecordsChangeds");

            migrationBuilder.DropColumn(
                name: "ProductDescription",
                table: "ProductRecordsChangeds");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "ProductRecordsChangeds");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "ProductRecordsChangeds");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrencyType",
                table: "ProductRecordsChangeds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "ProductRecordsChangeds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ProductRecordsChangeds",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductCategory",
                table: "ProductRecordsChangeds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductDescription",
                table: "ProductRecordsChangeds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "ProductRecordsChangeds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "ProductRecordsChangeds",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
