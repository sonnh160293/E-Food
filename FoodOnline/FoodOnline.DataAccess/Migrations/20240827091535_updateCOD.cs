using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodOnline.DataAccess.Migrations
{
    public partial class updateCOD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "Orders",
                newName: "IsCOD");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsCOD",
                table: "Orders",
                newName: "PaymentMethod");
        }
    }
}
