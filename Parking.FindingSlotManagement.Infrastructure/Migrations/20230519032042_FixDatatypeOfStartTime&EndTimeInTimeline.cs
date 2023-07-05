using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.FindingSlotManagement.Infrastructure.Migrations
{
    public partial class FixDatatypeOfStartTimeEndTimeInTimeline : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartTime",
                table: "TimeLine",
                type: "time",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndTime",
                table: "TimeLine",
                type: "time",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "TimeLine",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "TimeLine",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);
        }
    }
}
