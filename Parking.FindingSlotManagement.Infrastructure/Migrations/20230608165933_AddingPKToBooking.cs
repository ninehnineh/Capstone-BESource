using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.FindingSlotManagement.Infrastructure.Migrations
{
    public partial class AddingPKToBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey("PK_Booking", "Booking");

            migrationBuilder.AddPrimaryKey("pk_bookingkey", "Booking", new[] { "ParkingSlotId", "StartTime", "DateBook" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey("pk_bookingkey", "Booking");

            migrationBuilder.AddPrimaryKey("PK_Booking", "Booking", "Id");
        }
    }
}
