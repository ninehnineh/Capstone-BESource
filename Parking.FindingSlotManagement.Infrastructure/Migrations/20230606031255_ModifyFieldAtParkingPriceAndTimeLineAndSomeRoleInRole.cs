using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.FindingSlotManagement.Infrastructure.Migrations
{
    public partial class ModifyFieldAtParkingPriceAndTimeLineAndSomeRoleInRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<bool>(
                name: "HasPenaltyPrice",
                table: "ParkingPrice",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PenaltyPrice",
                table: "ParkingPrice",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "PenaltyPriceStepTime",
                table: "ParkingPrice",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StartingTime",
                table: "ParkingPrice",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                column: "Name",
                value: "Keeper");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "IsActive", "Name" },
                values: new object[] { 4, true, "Staff" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "HasPenaltyPrice",
                table: "ParkingPrice");

            migrationBuilder.DropColumn(
                name: "PenaltyPrice",
                table: "ParkingPrice");

            migrationBuilder.DropColumn(
                name: "PenaltyPriceStepTime",
                table: "ParkingPrice");

            migrationBuilder.DropColumn(
                name: "StartingTime",
                table: "ParkingPrice");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                column: "Name",
                value: "Staff");
        }
    }
}
