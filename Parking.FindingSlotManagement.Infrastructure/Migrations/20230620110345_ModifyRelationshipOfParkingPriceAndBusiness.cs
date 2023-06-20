using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.FindingSlotManagement.Infrastructure.Migrations
{
    public partial class ModifyRelationshipOfParkingPriceAndBusiness : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Business__ParkingPri__sdwq23dca",
                table: "ParkingPrice");

            migrationBuilder.AlterColumn<int>(
                name: "BusinessId",
                table: "ParkingPrice",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK__Business__ParkingPri",
                table: "ParkingPrice",
                column: "BusinessId",
                principalTable: "Business",
                principalColumn: "BusinessProfileId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Business__ParkingPri",
                table: "ParkingPrice");

            migrationBuilder.AlterColumn<int>(
                name: "BusinessId",
                table: "ParkingPrice",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK__Business__ParkingPri__sdwq23dca",
                table: "ParkingPrice",
                column: "BusinessId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
