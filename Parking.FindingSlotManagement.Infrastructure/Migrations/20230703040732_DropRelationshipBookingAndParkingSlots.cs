using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.FindingSlotManagement.Infrastructure.Migrations
{
    public partial class DropRelationshipBookingAndParkingSlots : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_ParkingSlots_ParkingSlotId",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_ParkingSlotId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "ParkingSlotId",
                table: "Booking");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParkingSlotId",
                table: "Booking",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_ParkingSlotId",
                table: "Booking",
                column: "ParkingSlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_ParkingSlots_ParkingSlotId",
                table: "Booking",
                column: "ParkingSlotId",
                principalTable: "ParkingSlots",
                principalColumn: "ParkingSlotId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
