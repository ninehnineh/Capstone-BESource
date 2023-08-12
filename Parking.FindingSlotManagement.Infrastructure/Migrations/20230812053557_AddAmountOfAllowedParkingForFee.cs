using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.FindingSlotManagement.Infrastructure.Migrations
{
    public partial class AddAmountOfAllowedParkingForFee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NumberOfParking",
                table: "Fees",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Fees",
                columns: new[] { "FeeId", "BusinessType", "Name", "NumberOfParking", "Price" },
                values: new object[] { 1, "Tư nhân", "Cước phí mặc định tư nhân", "1", 100000m });

            migrationBuilder.InsertData(
                table: "Fees",
                columns: new[] { "FeeId", "BusinessType", "Name", "NumberOfParking", "Price" },
                values: new object[] { 2, "Doanh nghiệp", "Cước phí mặc định doanh nghiệp", "Unlimited", 500000m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Fees",
                keyColumn: "FeeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Fees",
                keyColumn: "FeeId",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "NumberOfParking",
                table: "Fees");
        }
    }
}
