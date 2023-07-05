using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.FindingSlotManagement.Infrastructure.Migrations
{
    public partial class ModifyStartingTimeToInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE TimeLine ADD NewStartingTime INT");

            migrationBuilder.Sql("UPDATE TimeLine SET NewStartingTime = CAST(StartingTime AS INT)");

            migrationBuilder.Sql("ALTER TABLE TimeLine DROP COLUMN StartingTime");

            migrationBuilder.Sql("EXEC sp_rename 'TimeLine.NewStartingTime', 'StartingTime', 'COLUMN'");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, true, "Manager" },
                    { 2, true, "Staff" },
                    { 3, true, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Traffic",
                columns: new[] { "TrafficId", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, true, "Xe ô tô" },
                    { 2, true, "Xe máy" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("ALTER TABLE TimeLine ADD NewStartingTime DATETIME");

            migrationBuilder.Sql("UPDATE TimeLine SET NewStartingTime = CAST(StartingTime AS DATETIME)");

            migrationBuilder.Sql("ALTER TABLE TimeLine DROP COLUMN StartingTime");

            migrationBuilder.Sql("EXEC sp_rename 'TimeLine.NewStartingTime', 'StartingTime', 'COLUMN'");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Traffic",
                keyColumn: "TrafficId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Traffic",
                keyColumn: "TrafficId",
                keyValue: 2);
        }
    }
}
