using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagazineManagment.DAL.Migrations
{
    public partial class ProductSecondTableForChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Products");
        }
    }
}
