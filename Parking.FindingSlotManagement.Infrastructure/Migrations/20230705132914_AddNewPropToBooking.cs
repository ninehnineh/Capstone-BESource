using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.FindingSlotManagement.Infrastructure.Migrations
{
    public partial class AddNewPropToBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Parking");

            migrationBuilder.AddColumn<decimal>(
                name: "UnPaidMoney",
                table: "Booking",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnPaidMoney",
                table: "Booking");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Parking",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
