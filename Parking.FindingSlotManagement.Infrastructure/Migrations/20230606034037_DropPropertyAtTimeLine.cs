using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.FindingSlotManagement.Infrastructure.Migrations
{
    public partial class DropPropertyAtTimeLine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasPenaltyPrice",
                table: "TimeLine");

            migrationBuilder.DropColumn(
                name: "PenaltyPrice",
                table: "TimeLine");

            migrationBuilder.DropColumn(
                name: "PenaltyPriceStepTime",
                table: "TimeLine");

            migrationBuilder.DropColumn(
                name: "StartingTime",
                table: "TimeLine");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Roles",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<bool>(
                name: "HasPenaltyPrice",
                table: "TimeLine",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PenaltyPrice",
                table: "TimeLine",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "PenaltyPriceStepTime",
                table: "TimeLine",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StartingTime",
                table: "TimeLine",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Roles",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);
        }
    }
}
