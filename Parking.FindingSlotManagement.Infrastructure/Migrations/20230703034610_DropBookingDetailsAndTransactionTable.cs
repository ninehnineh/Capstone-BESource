using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.FindingSlotManagement.Infrastructure.Migrations
{
    public partial class DropBookingDetailsAndTransactionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Booking_BookingParkingSlotId_BookingStartTime_BookingDateBook",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_WalletId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "Transaction");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_WalletId",
                table: "Transaction",
                newName: "IX_Transaction_WalletId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_BookingParkingSlotId_BookingStartTime_BookingDateBook",
                table: "Transaction",
                newName: "IX_Transaction_BookingParkingSlotId_BookingStartTime_BookingDateBook");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Booking_BookingParkingSlotId_BookingStartTime_BookingDateBook",
                table: "Transaction",
                columns: new[] { "BookingParkingSlotId", "BookingStartTime", "BookingDateBook" },
                principalTable: "Booking",
                principalColumns: new[] { "ParkingSlotId", "StartTime", "DateBook" });

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Wallets_WalletId",
                table: "Transaction",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "WalletId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Booking_BookingParkingSlotId_BookingStartTime_BookingDateBook",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Wallets_WalletId",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction");

            migrationBuilder.RenameTable(
                name: "Transaction",
                newName: "Transactions");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_WalletId",
                table: "Transactions",
                newName: "IX_Transactions_WalletId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_BookingParkingSlotId_BookingStartTime_BookingDateBook",
                table: "Transactions",
                newName: "IX_Transactions_BookingParkingSlotId_BookingStartTime_BookingDateBook");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "TransactionId");

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
    }
}
