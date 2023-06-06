using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.FindingSlotManagement.Infrastructure.Migrations
{
    public partial class AddedPropertyToParkingPriceAndRemoveAtTimeLine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtraTimeStep",
                table: "TimeLine");

            migrationBuilder.DropColumn(
                name: "IsExtrafee",
                table: "TimeLine");

            migrationBuilder.AddColumn<float>(
                name: "ExtraTimeStep",
                table: "ParkingPrice",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsExtrafee",
                table: "ParkingPrice",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtraTimeStep",
                table: "ParkingPrice");

            migrationBuilder.DropColumn(
                name: "IsExtrafee",
                table: "ParkingPrice");

            migrationBuilder.AddColumn<float>(
                name: "ExtraTimeStep",
                table: "TimeLine",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsExtrafee",
                table: "TimeLine",
                type: "bit",
                nullable: true);
        }
    }
}
