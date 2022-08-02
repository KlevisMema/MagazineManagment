using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagazineManagment.DAL.Migrations
{
    public partial class ProductRecordsChangedPropertyQunatityBeforeRemovalAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QunatityBeforeRemoval",
                table: "ProductRecordsChangeds",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QunatityBeforeRemoval",
                table: "ProductRecordsChangeds");
        }
    }
}
