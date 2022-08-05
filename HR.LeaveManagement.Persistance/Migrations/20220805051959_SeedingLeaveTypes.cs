using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR.LeaveManagement.Persistence.Migrations
{
    public partial class SeedingLeaveTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LeaveTypes",
                columns: new[] { "Id", "CreatedBy", "DateCreated", "DefaultDays", "LastModifiedBy", "LastModifiedDate", "Name" },
                values: new object[] { 1, "Amin", new DateTime(2022, 8, 4, 22, 19, 59, 555, DateTimeKind.Local).AddTicks(6230), 10, "Amin", new DateTime(2022, 8, 4, 22, 19, 59, 555, DateTimeKind.Local).AddTicks(6268), "Vacation" });

            migrationBuilder.InsertData(
                table: "LeaveTypes",
                columns: new[] { "Id", "CreatedBy", "DateCreated", "DefaultDays", "LastModifiedBy", "LastModifiedDate", "Name" },
                values: new object[] { 2, "Amin", new DateTime(2022, 8, 4, 22, 19, 59, 555, DateTimeKind.Local).AddTicks(6271), 12, "Amin", new DateTime(2022, 8, 4, 22, 19, 59, 555, DateTimeKind.Local).AddTicks(6272), "Sick" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
