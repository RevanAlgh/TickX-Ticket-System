using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticket.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedResetPass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetPasswordToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetPasswordTokenExpiration",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "ResetPasswordToken", "ResetPasswordTokenExpiration", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 14, 14, 21, 5, 597, DateTimeKind.Utc).AddTicks(9111), "$2a$11$36fVmQ0YFQMGQliHFVuk8e7X6dS7igSRxdRXA/yRyC3jWX8GpRiVm", null, null, new DateTime(2024, 8, 14, 14, 21, 5, 597, DateTimeKind.Utc).AddTicks(9115) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetPasswordToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ResetPasswordTokenExpiration",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 11, 18, 24, 45, 590, DateTimeKind.Utc).AddTicks(4432), "$2a$11$qxrJ4j3/n/mp40mLpqEVU.HmdBOP5szdb9DbHGQgdUyE3.1CMQIgq", new DateTime(2024, 8, 11, 18, 24, 45, 590, DateTimeKind.Utc).AddTicks(4438) });
        }
    }
}
