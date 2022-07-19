using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagazineManagment.DAL.Migrations
{
    public partial class Added_ProductInStoctPropInProductModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductInStock",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductInStock",
                table: "Products");
        }
    }
}
