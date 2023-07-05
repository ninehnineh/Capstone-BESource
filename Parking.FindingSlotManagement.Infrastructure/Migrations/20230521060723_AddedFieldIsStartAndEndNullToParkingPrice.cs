using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.FindingSlotManagement.Infrastructure.Migrations
{
    public partial class AddedFieldIsStartAndEndNullToParkingPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "TimeLine",
                type: "money",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "money",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsStartAndEndNull",
                table: "ParkingPrice",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsStartAndEndNull",
                table: "ParkingPrice");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "TimeLine",
                type: "money",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "money");
        }
    }
}
