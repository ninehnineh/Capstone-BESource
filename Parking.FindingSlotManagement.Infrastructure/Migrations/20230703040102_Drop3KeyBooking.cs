using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.FindingSlotManagement.Infrastructure.Migrations
{
    public partial class Drop3KeyBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_bookingkeyV2",
                table: "Booking");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Booking",
                table: "Booking",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_ParkingSlotId",
                table: "Booking",
                column: "ParkingSlotId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Booking",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_ParkingSlotId",
                table: "Booking");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Booking__1BDD09E6ABAB9F2E",
                table: "Booking",
                columns: new[] { "ParkingSlotId", "StartTime", "DateBook" });
        }
    }
}
