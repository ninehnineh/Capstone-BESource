using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.FindingSlotManagement.Infrastructure.Migrations
{
    public partial class ModifyVehicelInforToVehicel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Booking__Vehicle__4F7CD00D",
                table: "Booking");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VehicleInfor",
                table: "VehicleInfor");

            migrationBuilder.RenameTable(
                name: "VehicleInfor",
                newName: "Vehicle");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicle",
                table: "Vehicle",
                column: "VehicleInforId");

            migrationBuilder.AddForeignKey(
                name: "FK__Booking__Vehicle",
                table: "Booking",
                column: "VehicleInforID",
                principalTable: "Vehicle",
                principalColumn: "VehicleInforId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Booking__Vehicle",
                table: "Booking");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicle",
                table: "Vehicle");

            migrationBuilder.RenameTable(
                name: "Vehicle",
                newName: "VehicleInfor");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehicleInfor",
                table: "VehicleInfor",
                column: "VehicleInforId");

            migrationBuilder.AddForeignKey(
                name: "FK__Booking__Vehicle__4F7CD00D",
                table: "Booking",
                column: "VehicleInforID",
                principalTable: "VehicleInfor",
                principalColumn: "VehicleInforId");
        }
    }
}
