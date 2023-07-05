using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.FindingSlotManagement.Infrastructure.Migrations
{
    public partial class AddTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Traffic_Parkingpri",
                table: "ParkingPrice");

            migrationBuilder.DropForeignKey(
                name: "FK__ParkingSl__Traff__534D60F1",
                table: "ParkingSlots");

            migrationBuilder.DropForeignKey(
                name: "FK__VehicleIn__Traff__4BAC3F29",
                table: "VehicleInfor");

            migrationBuilder.DropForeignKey(
                name: "FK__VnPay__BusinessId__2A4B4B5E",
                table: "VnPay");

            migrationBuilder.DropTable(
                name: "FieldWorkImg");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Traffic",
                table: "Traffic");

            migrationBuilder.DropColumn(
                name: "ActualPrice",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "TmnCodeVnPay",
                table: "Booking");

            migrationBuilder.RenameTable(
                name: "Traffic",
                newName: "VehicleType");

            migrationBuilder.RenameColumn(
                name: "BusinessId",
                table: "VnPay",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_VnPay_BusinessId",
                table: "VnPay",
                newName: "IX_userId_VnPay");

            migrationBuilder.AddColumn<int>(
                name: "FeeId",
                table: "Business",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Business",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Booking_BookingID",
                table: "Booking",
                column: "BookingID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehicleType",
                table: "VehicleType",
                column: "TrafficId");

            migrationBuilder.CreateTable(
                name: "ApproveParkings",
                columns: table => new
                {
                    ApproveParkingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StaffId = table.Column<int>(type: "int", nullable: true),
                    ParkingId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApproveParkings", x => x.ApproveParkingId);
                    table.ForeignKey(
                        name: "FK_Parking_ApproveParkings",
                        column: x => x.ParkingId,
                        principalTable: "Parking",
                        principalColumn: "ParkingId");
                    table.ForeignKey(
                        name: "FK_User_ApproveParkings",
                        column: x => x.StaffId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "BookedSlots",
                columns: table => new
                {
                    BookedSlotId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ParkingSlotId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookedSlots", x => x.BookedSlotId);
                    table.ForeignKey(
                        name: "FK_Parkingslot_BookedSlots",
                        column: x => x.ParkingSlotId,
                        principalTable: "ParkingSlots",
                        principalColumn: "ParkingSlotId");
                });

            migrationBuilder.CreateTable(
                name: "Fees",
                columns: table => new
                {
                    FeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fees", x => x.FeeId);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    WalletId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Debt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.WalletId);
                    table.ForeignKey(
                        name: "FK_User_Wallets",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "FieldWorkParkingImgs",
                columns: table => new
                {
                    FieldWorkParkingImgId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApproveParkingId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldWorkParkingImgs", x => x.FieldWorkParkingImgId);
                    table.ForeignKey(
                        name: "FK_ApprovePar_FieldWorkPas",
                        column: x => x.ApproveParkingId,
                        principalTable: "ApproveParkings",
                        principalColumn: "ApproveParkingId");
                });

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    BillId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BusinessId = table.Column<int>(type: "int", nullable: true),
                    WalletId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.BillId);
                    table.ForeignKey(
                        name: "FK_businessPro_Bills",
                        column: x => x.BusinessId,
                        principalTable: "Business",
                        principalColumn: "BusinessProfileId");
                    table.ForeignKey(
                        name: "FK_Wallet_Bills",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "WalletId");
                });

            migrationBuilder.CreateTable(
                name: "BookingPayments",
                columns: table => new
                {
                    BookingPaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WalletId = table.Column<int>(type: "int", nullable: true),
                    BookingId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingPayments", x => x.BookingPaymentId);
                    table.ForeignKey(
                        name: "FK_Booking_BookingPayments",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "BookingID");
                    table.ForeignKey(
                        name: "FK_Wallet_BookingPayments",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "WalletId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Business_FeeId",
                table: "Business",
                column: "FeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ApproveParkings_ParkingId",
                table: "ApproveParkings",
                column: "ParkingId");

            migrationBuilder.CreateIndex(
                name: "IX_ApproveParkings_StaffId",
                table: "ApproveParkings",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_BusinessId",
                table: "Bills",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_WalletId",
                table: "Bills",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_BookedSlots_ParkingSlotId",
                table: "BookedSlots",
                column: "ParkingSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingPayments_BookingId",
                table: "BookingPayments",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingPayments_WalletId",
                table: "BookingPayments",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldWorkParkingImgs_ApproveParkingId",
                table: "FieldWorkParkingImgs",
                column: "ApproveParkingId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_UserId",
                table: "Wallets",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fee_BusinessProfiles",
                table: "Business",
                column: "FeeId",
                principalTable: "Fees",
                principalColumn: "FeeId");

            migrationBuilder.AddForeignKey(
                name: "FK__VehicleTy_Parkingpri",
                table: "ParkingPrice",
                column: "TrafficId",
                principalTable: "VehicleType",
                principalColumn: "TrafficId");

            migrationBuilder.AddForeignKey(
                name: "FK__ParkingSl__Traff",
                table: "ParkingSlots",
                column: "TrafficID",
                principalTable: "VehicleType",
                principalColumn: "TrafficId");

            migrationBuilder.AddForeignKey(
                name: "FK__VehicleIn__Traff",
                table: "VehicleInfor",
                column: "TrafficID",
                principalTable: "VehicleType",
                principalColumn: "TrafficId");

            migrationBuilder.AddForeignKey(
                name: "FK__VnPay__User",
                table: "VnPay",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fee_BusinessProfiles",
                table: "Business");

            migrationBuilder.DropForeignKey(
                name: "FK__VehicleTy_Parkingpri",
                table: "ParkingPrice");

            migrationBuilder.DropForeignKey(
                name: "FK__ParkingSl__Traff",
                table: "ParkingSlots");

            migrationBuilder.DropForeignKey(
                name: "FK__VehicleIn__Traff",
                table: "VehicleInfor");

            migrationBuilder.DropForeignKey(
                name: "FK__VnPay__User",
                table: "VnPay");

            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "BookedSlots");

            migrationBuilder.DropTable(
                name: "BookingPayments");

            migrationBuilder.DropTable(
                name: "Fees");

            migrationBuilder.DropTable(
                name: "FieldWorkParkingImgs");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropTable(
                name: "ApproveParkings");

            migrationBuilder.DropIndex(
                name: "IX_Business_FeeId",
                table: "Business");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Booking_BookingID",
                table: "Booking");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VehicleType",
                table: "VehicleType");

            migrationBuilder.DropColumn(
                name: "FeeId",
                table: "Business");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Business");

            migrationBuilder.RenameTable(
                name: "VehicleType",
                newName: "Traffic");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "VnPay",
                newName: "BusinessId");

            migrationBuilder.RenameIndex(
                name: "IX_userId_VnPay",
                table: "VnPay",
                newName: "IX_VnPay_BusinessId");

            migrationBuilder.AddColumn<decimal>(
                name: "ActualPrice",
                table: "Booking",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Booking",
                type: "varchar(225)",
                unicode: false,
                maxLength: 225,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TmnCodeVnPay",
                table: "Booking",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Traffic",
                table: "Traffic",
                column: "TrafficId");

            migrationBuilder.CreateTable(
                name: "FieldWorkImg",
                columns: table => new
                {
                    FieldWorkImgId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessProfileId = table.Column<int>(type: "int", nullable: false),
                    ImgUrl = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldWorkImg", x => x.FieldWorkImgId);
                    table.ForeignKey(
                        name: "FK_FieldWorkImg_Business",
                        column: x => x.BusinessProfileId,
                        principalTable: "Business",
                        principalColumn: "BusinessProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FieldWorkImg_BusinessProfileId",
                table: "FieldWorkImg",
                column: "BusinessProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK__Traffic_Parkingpri",
                table: "ParkingPrice",
                column: "TrafficId",
                principalTable: "Traffic",
                principalColumn: "TrafficId");

            migrationBuilder.AddForeignKey(
                name: "FK__ParkingSl__Traff__534D60F1",
                table: "ParkingSlots",
                column: "TrafficID",
                principalTable: "Traffic",
                principalColumn: "TrafficId");

            migrationBuilder.AddForeignKey(
                name: "FK__VehicleIn__Traff__4BAC3F29",
                table: "VehicleInfor",
                column: "TrafficID",
                principalTable: "Traffic",
                principalColumn: "TrafficId");

            migrationBuilder.AddForeignKey(
                name: "FK__VnPay__BusinessId__2A4B4B5E",
                table: "VnPay",
                column: "BusinessId",
                principalTable: "Business",
                principalColumn: "BusinessProfileId");
        }
    }
}
