using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticket.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedRemindertoticket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReminderLevel",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 17, 4, 33, 36, 485, DateTimeKind.Utc).AddTicks(4447), "$2a$11$aZ1s6JyM4Co2QmN6iIaBH.DyrLGL39hOs5hAy/YbhVTItRcKWRcG.", new DateTime(2024, 8, 17, 4, 33, 36, 485, DateTimeKind.Utc).AddTicks(4457) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReminderLevel",
                table: "Tickets");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 14, 14, 21, 5, 597, DateTimeKind.Utc).AddTicks(9111), "$2a$11$36fVmQ0YFQMGQliHFVuk8e7X6dS7igSRxdRXA/yRyC3jWX8GpRiVm", new DateTime(2024, 8, 14, 14, 21, 5, 597, DateTimeKind.Utc).AddTicks(9115) });
        }
    }
}
