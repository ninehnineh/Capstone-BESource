using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.FindingSlotManagement.Infrastructure.Migrations
{
    public partial class RemovedRelationshipParkingSlotAndParking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__ParkingSl__Parki__5535A963",
                table: "ParkingSlots");

            migrationBuilder.DropIndex(
                name: "IX_ParkingSlots_ParkingID",
                table: "ParkingSlots");

            migrationBuilder.DropColumn(
                name: "ParkingID",
                table: "ParkingSlots");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParkingID",
                table: "ParkingSlots",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSlots_ParkingID",
                table: "ParkingSlots",
                column: "ParkingID");

            migrationBuilder.AddForeignKey(
                name: "FK__ParkingSl__Parki__5535A963",
                table: "ParkingSlots",
                column: "ParkingID",
                principalTable: "Parking",
                principalColumn: "ParkingId");
        }
    }
}
