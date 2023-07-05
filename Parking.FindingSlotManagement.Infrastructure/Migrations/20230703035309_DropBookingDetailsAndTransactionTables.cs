using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.FindingSlotManagement.Infrastructure.Migrations
{
    public partial class DropBookingDetailsAndTransactionTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingDetails");

            migrationBuilder.DropTable(
                name: "Transaction");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookingDetails",
                columns: table => new
                {
                    BookingDetailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingParkingSlotId = table.Column<int>(type: "int", nullable: true),
                    TimeSlotId = table.Column<int>(type: "int", nullable: true),
                    BookingStartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BookingDateBook = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BookingId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingDetails", x => x.BookingDetailsId);
                    table.ForeignKey(
                        name: "FK_BookingDetails_Booking_BookingParkingSlotId_BookingStartTime_BookingDateBook",
                        columns: x => new { x.BookingParkingSlotId, x.BookingStartTime, x.BookingDateBook },
                        principalTable: "Booking",
                        principalColumns: new[] { "ParkingSlotId", "StartTime", "DateBook" });
                    table.ForeignKey(
                        name: "FK_BookingDetails_TimeSlot_TimeSlotId",
                        column: x => x.TimeSlotId,
                        principalTable: "TimeSlot",
                        principalColumn: "TimeSlotId");
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingParkingSlotId = table.Column<int>(type: "int", nullable: true),
                    WalletId = table.Column<int>(type: "int", nullable: true),
                    BookingStartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BookingDateBook = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BookingId = table.Column<int>(type: "int", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transaction_Booking_BookingParkingSlotId_BookingStartTime_BookingDateBook",
                        columns: x => new { x.BookingParkingSlotId, x.BookingStartTime, x.BookingDateBook },
                        principalTable: "Booking",
                        principalColumns: new[] { "ParkingSlotId", "StartTime", "DateBook" });
                    table.ForeignKey(
                        name: "FK_Transaction_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "WalletId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_BookingParkingSlotId_BookingStartTime_BookingDateBook",
                table: "BookingDetails",
                columns: new[] { "BookingParkingSlotId", "BookingStartTime", "BookingDateBook" });

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_TimeSlotId",
                table: "BookingDetails",
                column: "TimeSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_BookingParkingSlotId_BookingStartTime_BookingDateBook",
                table: "Transaction",
                columns: new[] { "BookingParkingSlotId", "BookingStartTime", "BookingDateBook" });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_WalletId",
                table: "Transaction",
                column: "WalletId");
        }
    }
}
