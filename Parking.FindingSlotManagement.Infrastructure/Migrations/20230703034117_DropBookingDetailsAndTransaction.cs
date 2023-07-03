using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.FindingSlotManagement.Infrastructure.Migrations
{
    public partial class DropBookingDetailsAndTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Booking__BookingDetailss",
                table: "BookingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK__TimeSlot__BookingDetails",
                table: "BookingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_BookingPaymentss",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Wallet_BookingPayments",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_BookingDetails_BookingId",
                table: "BookingDetails");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Booking_BookingID",
                table: "Booking");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_BookingId",
                table: "Transaction");

            migrationBuilder.RenameTable(
                name: "Transaction",
                newName: "Transactions");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_WalletId",
                table: "Transactions",
                newName: "IX_Transactions_WalletId");

            migrationBuilder.AddColumn<DateTime>(
                name: "BookingDateBook",
                table: "BookingDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BookingParkingSlotId",
                table: "BookingDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BookingStartTime",
                table: "BookingDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BookingDateBook",
                table: "Transactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BookingParkingSlotId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BookingStartTime",
                table: "Transactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_BookingParkingSlotId_BookingStartTime_BookingDateBook",
                table: "BookingDetails",
                columns: new[] { "BookingParkingSlotId", "BookingStartTime", "BookingDateBook" });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BookingParkingSlotId_BookingStartTime_BookingDateBook",
                table: "Transactions",
                columns: new[] { "BookingParkingSlotId", "BookingStartTime", "BookingDateBook" });

            migrationBuilder.AddForeignKey(
                name: "FK_BookingDetails_Booking_BookingParkingSlotId_BookingStartTime_BookingDateBook",
                table: "BookingDetails",
                columns: new[] { "BookingParkingSlotId", "BookingStartTime", "BookingDateBook" },
                principalTable: "Booking",
                principalColumns: new[] { "ParkingSlotId", "StartTime", "DateBook" });

            migrationBuilder.AddForeignKey(
                name: "FK_BookingDetails_TimeSlot_TimeSlotId",
                table: "BookingDetails",
                column: "TimeSlotId",
                principalTable: "TimeSlot",
                principalColumn: "TimeSlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Booking_BookingParkingSlotId_BookingStartTime_BookingDateBook",
                table: "Transactions",
                columns: new[] { "BookingParkingSlotId", "BookingStartTime", "BookingDateBook" },
                principalTable: "Booking",
                principalColumns: new[] { "ParkingSlotId", "StartTime", "DateBook" });

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_WalletId",
                table: "Transactions",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "WalletId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingDetails_Booking_BookingParkingSlotId_BookingStartTime_BookingDateBook",
                table: "BookingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingDetails_TimeSlot_TimeSlotId",
                table: "BookingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Booking_BookingParkingSlotId_BookingStartTime_BookingDateBook",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_WalletId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_BookingDetails_BookingParkingSlotId_BookingStartTime_BookingDateBook",
                table: "BookingDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_BookingParkingSlotId_BookingStartTime_BookingDateBook",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "BookingDateBook",
                table: "BookingDetails");

            migrationBuilder.DropColumn(
                name: "BookingParkingSlotId",
                table: "BookingDetails");

            migrationBuilder.DropColumn(
                name: "BookingStartTime",
                table: "BookingDetails");

            migrationBuilder.DropColumn(
                name: "BookingDateBook",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "BookingParkingSlotId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "BookingStartTime",
                table: "Transactions");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "Transaction");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_WalletId",
                table: "Transaction",
                newName: "IX_Transaction_WalletId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Booking_BookingID",
                table: "Booking",
                column: "BookingID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_BookingId",
                table: "BookingDetails",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_BookingId",
                table: "Transaction",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK__Booking__BookingDetailss",
                table: "BookingDetails",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "BookingID");

            migrationBuilder.AddForeignKey(
                name: "FK__TimeSlot__BookingDetails",
                table: "BookingDetails",
                column: "TimeSlotId",
                principalTable: "TimeSlot",
                principalColumn: "TimeSlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_BookingPaymentss",
                table: "Transaction",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "BookingID");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallet_BookingPayments",
                table: "Transaction",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "WalletId");
        }
    }
}
