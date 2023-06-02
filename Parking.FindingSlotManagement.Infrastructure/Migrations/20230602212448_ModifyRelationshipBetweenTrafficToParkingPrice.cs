using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.FindingSlotManagement.Infrastructure.Migrations
{
    public partial class ModifyRelationshipBetweenTrafficToParkingPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timeline_Traffic",
                table: "TimeLine");

            migrationBuilder.DropIndex(
                name: "IX_TimeLine_TrafficId",
                table: "TimeLine");

            migrationBuilder.DropColumn(
                name: "TrafficId",
                table: "TimeLine");

            migrationBuilder.RenameColumn(
                name: "IsStartAndEndNull",
                table: "ParkingPrice",
                newName: "IsWholeDay");

            migrationBuilder.AddColumn<int>(
                name: "TrafficId",
                table: "ParkingPrice",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParkingPrice_TrafficId",
                table: "ParkingPrice",
                column: "TrafficId");

            migrationBuilder.AddForeignKey(
                name: "FK__Traffic_Parkingpri",
                table: "ParkingPrice",
                column: "TrafficId",
                principalTable: "Traffic",
                principalColumn: "TrafficId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Traffic_Parkingpri",
                table: "ParkingPrice");

            migrationBuilder.DropIndex(
                name: "IX_ParkingPrice_TrafficId",
                table: "ParkingPrice");

            migrationBuilder.DropColumn(
                name: "TrafficId",
                table: "ParkingPrice");

            migrationBuilder.RenameColumn(
                name: "IsWholeDay",
                table: "ParkingPrice",
                newName: "IsStartAndEndNull");

            migrationBuilder.AddColumn<int>(
                name: "TrafficId",
                table: "TimeLine",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeLine_TrafficId",
                table: "TimeLine",
                column: "TrafficId");

            migrationBuilder.AddForeignKey(
                name: "FK_Timeline_Traffic",
                table: "TimeLine",
                column: "TrafficId",
                principalTable: "Traffic",
                principalColumn: "TrafficId");
        }
    }
}
