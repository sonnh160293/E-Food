using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodOnline.DataAccess.Migrations
{
    public partial class updateIsConfirmColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsConfirm",
                table: "Orders",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConfirm",
                table: "Orders");
        }
    }
}
