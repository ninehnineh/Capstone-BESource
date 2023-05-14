using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.FindingSlotManagement.Infrastructure.Migrations
{
    public partial class AddedRelationshipBetweenUserAndParkingPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ParkingPrice_BusinessId",
                table: "ParkingPrice",
                column: "BusinessId");

            migrationBuilder.AddForeignKey(
                name: "FK__Business__ParkingPri__sdwq23dca",
                table: "ParkingPrice",
                column: "BusinessId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Business__ParkingPri__sdwq23dca",
                table: "ParkingPrice");

            migrationBuilder.DropIndex(
                name: "IX_ParkingPrice_BusinessId",
                table: "ParkingPrice");
        }
    }
}
