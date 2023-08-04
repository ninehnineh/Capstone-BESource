using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.FindingSlotManagement.Infrastructure.Migrations
{
    public partial class AddmorepropsinParkingSlotAndBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BanCount",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBackup",
                table: "ParkingSlots",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRating",
                table: "Booking",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BanCount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsBackup",
                table: "ParkingSlots");

            migrationBuilder.DropColumn(
                name: "IsRating",
                table: "Booking");
        }
    }
}
