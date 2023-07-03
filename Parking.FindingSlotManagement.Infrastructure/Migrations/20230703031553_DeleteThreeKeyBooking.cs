using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.FindingSlotManagement.Infrastructure.Migrations
{
    public partial class DeleteThreeKeyBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_ParkingSlots_ParkingSlotID",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK__Booking__BookingDetails",
                table: "BookingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_BookingPayments",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "AK_Booking_BookingIDsas",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "UQ__Booking__3214EC2628BBAE14",
                table: "Booking");

            migrationBuilder.RenameColumn(
                name: "ParkingSlotID",
                table: "Booking",
                newName: "ParkingSlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_ParkingSlots_ParkingSlotId",
                table: "Booking",
                column: "ParkingSlotId",
                principalTable: "ParkingSlots",
                principalColumn: "ParkingSlotId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Booking__BookingDetailss",
                table: "BookingDetails",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "BookingID");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_BookingPaymentss",
                table: "Transaction",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "BookingID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_ParkingSlots_ParkingSlotId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK__Booking__BookingDetailss",
                table: "BookingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_BookingPaymentss",
                table: "Transaction");

            migrationBuilder.RenameColumn(
                name: "ParkingSlotId",
                table: "Booking",
                newName: "ParkingSlotID");

            migrationBuilder.CreateIndex(
                name: "AK_Booking_BookingIDsas",
                table: "Booking",
                column: "BookingID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Booking__3214EC2628BBAE14",
                table: "Booking",
                column: "BookingID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_ParkingSlots_ParkingSlotID",
                table: "Booking",
                column: "ParkingSlotID",
                principalTable: "ParkingSlots",
                principalColumn: "ParkingSlotId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Booking__BookingDetails",
                table: "BookingDetails",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "BookingID");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_BookingPayments",
                table: "Transaction",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "BookingID");
        }
    }
}
