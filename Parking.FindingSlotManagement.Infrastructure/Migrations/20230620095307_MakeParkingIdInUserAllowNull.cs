using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.FindingSlotManagement.Infrastructure.Migrations
{
    public partial class MakeParkingIdInUserAllowNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Parking__Users",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "ParkingId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK__Parking__Users",
                table: "Users",
                column: "ParkingId",
                principalTable: "Parking",
                principalColumn: "ParkingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Parking__Users",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "ParkingId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK__Parking__Users",
                table: "Users",
                column: "ParkingId",
                principalTable: "Parking",
                principalColumn: "ParkingId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
