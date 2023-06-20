using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.FindingSlotManagement.Infrastructure.Migrations
{
    public partial class ChangeBusinessRulesUserAndParking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldWorkImg_BusinessProfiles",
                table: "FieldWorkImg");

            migrationBuilder.DropForeignKey(
                name: "FK__VnPay__ManagerID__2A4B4B5E",
                table: "VnPay");

            migrationBuilder.DropTable(
                name: "OTPs");

            migrationBuilder.DropTable(
                name: "StaffParking");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusinessProfiles",
                table: "BusinessProfiles");

            migrationBuilder.DropColumn(
                name: "ManagerID",
                table: "Parking");

            migrationBuilder.RenameTable(
                name: "BusinessProfiles",
                newName: "Business");

            migrationBuilder.RenameColumn(
                name: "ManagerID",
                table: "VnPay",
                newName: "BusinessId");

            migrationBuilder.RenameIndex(
                name: "IX_VnPay_ManagerID",
                table: "VnPay",
                newName: "IX_VnPay_BusinessId");

            migrationBuilder.AddColumn<int>(
                name: "ParkingId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BusinessId",
                table: "Parking",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Business",
                table: "Business",
                column: "BusinessProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ParkingId",
                table: "Users",
                column: "ParkingId");

            migrationBuilder.CreateIndex(
                name: "IX_Parking_BusinessId",
                table: "Parking",
                column: "BusinessId");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldWorkImg_Business",
                table: "FieldWorkImg",
                column: "BusinessProfileId",
                principalTable: "Business",
                principalColumn: "BusinessProfileId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__BusiessPro__Parking",
                table: "Parking",
                column: "BusinessId",
                principalTable: "Business",
                principalColumn: "BusinessProfileId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Parking__Users",
                table: "Users",
                column: "ParkingId",
                principalTable: "Parking",
                principalColumn: "ParkingId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__VnPay__BusinessId__2A4B4B5E",
                table: "VnPay",
                column: "BusinessId",
                principalTable: "Business",
                principalColumn: "BusinessProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldWorkImg_Business",
                table: "FieldWorkImg");

            migrationBuilder.DropForeignKey(
                name: "FK__BusiessPro__Parking",
                table: "Parking");

            migrationBuilder.DropForeignKey(
                name: "FK__Parking__Users",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK__VnPay__BusinessId__2A4B4B5E",
                table: "VnPay");

            migrationBuilder.DropIndex(
                name: "IX_Users_ParkingId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Parking_BusinessId",
                table: "Parking");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Business",
                table: "Business");

            migrationBuilder.DropColumn(
                name: "ParkingId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Parking");

            migrationBuilder.RenameTable(
                name: "Business",
                newName: "BusinessProfiles");

            migrationBuilder.RenameColumn(
                name: "BusinessId",
                table: "VnPay",
                newName: "ManagerID");

            migrationBuilder.RenameIndex(
                name: "IX_VnPay_BusinessId",
                table: "VnPay",
                newName: "IX_VnPay_ManagerID");

            migrationBuilder.AddColumn<int>(
                name: "ManagerID",
                table: "Parking",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusinessProfiles",
                table: "BusinessProfiles",
                column: "BusinessProfileId");

            migrationBuilder.CreateTable(
                name: "OTPs",
                columns: table => new
                {
                    OTPID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "char(6)", unicode: false, fixedLength: true, maxLength: 6, nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OTPs", x => x.OTPID);
                    table.ForeignKey(
                        name: "FK__OTP__UserID",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffParking",
                columns: table => new
                {
                    StaffParkingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParkingID = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffParking", x => x.StaffParkingId);
                    table.ForeignKey(
                        name: "FK__StaffPark__Parki__398D8EEE",
                        column: x => x.ParkingID,
                        principalTable: "Parking",
                        principalColumn: "ParkingId");
                    table.ForeignKey(
                        name: "FK__StaffPark__UserI__38996AB5",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OTPs_UserId",
                table: "OTPs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffParking_ParkingID",
                table: "StaffParking",
                column: "ParkingID");

            migrationBuilder.CreateIndex(
                name: "IX_StaffParking_UserID",
                table: "StaffParking",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldWorkImg_BusinessProfiles",
                table: "FieldWorkImg",
                column: "BusinessProfileId",
                principalTable: "BusinessProfiles",
                principalColumn: "BusinessProfileId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__VnPay__ManagerID__2A4B4B5E",
                table: "VnPay",
                column: "ManagerID",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
