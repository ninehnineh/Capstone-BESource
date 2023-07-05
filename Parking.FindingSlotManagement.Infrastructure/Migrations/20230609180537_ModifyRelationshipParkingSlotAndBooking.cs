using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.FindingSlotManagement.Infrastructure.Migrations
{
    public partial class ModifyRelationshipParkingSlotAndBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__ParkingSl__Booki__5629CD9C",
                table: "ParkingSlots");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_ParkingSlots_BookingID",
                table: "ParkingSlots");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Booking_BookingID",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "BookingID",
                table: "ParkingSlots");

            migrationBuilder.DropColumn(
                name: "QRCodeText",
                table: "Booking");

            migrationBuilder.AddForeignKey(
                name: "FK__Booki__ParkSlo",
                table: "Booking",
                column: "ParkingSlotID",
                principalTable: "ParkingSlots",
                principalColumn: "ParkingSlotId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Booki__ParkSlo",
                table: "Booking");

            migrationBuilder.AddColumn<int>(
                name: "BookingID",
                table: "ParkingSlots",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QRCodeText",
                table: "Booking",
                type: "varchar(225)",
                unicode: false,
                maxLength: 225,
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Booking_BookingID",
                table: "Booking",
                column: "BookingID");

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingID = table.Column<int>(type: "int", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(225)", maxLength: 225, nullable: true),
                    SentTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    Tiltle = table.Column<string>(type: "nvarchar(225)", maxLength: 225, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK__Notificat__Booki__59063A47",
                        column: x => x.BookingID,
                        principalTable: "Booking",
                        principalColumn: "BookingID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSlots_BookingID",
                table: "ParkingSlots",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_BookingID",
                table: "Notifications",
                column: "BookingID");

            migrationBuilder.AddForeignKey(
                name: "FK__ParkingSl__Booki__5629CD9C",
                table: "ParkingSlots",
                column: "BookingID",
                principalTable: "Booking",
                principalColumn: "BookingID");
        }
    }
}
