using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.FindingSlotManagement.Infrastructure.Migrations
{
    public partial class ModifyTypeOfDateBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey("pk_bookingkey", "Booking");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateBook",
                table: "Booking",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddPrimaryKey("pk_bookingkey", "Booking", new[] { "ParkingSlotId", "StartTime", "DateBook" });

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateBook",
                table: "Booking",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
