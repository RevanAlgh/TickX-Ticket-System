using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticket.Data.Migrations
{
    /// <inheritdoc />
    public partial class makingclosebynull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 11, 10, 43, 31, 739, DateTimeKind.Utc).AddTicks(3170), "$2a$11$sv.mye.QbDvhx1mniMCjue9af/oOoeofve3tu/rcFaXmuvWREGsrW", new DateTime(2024, 8, 11, 10, 43, 31, 739, DateTimeKind.Utc).AddTicks(3176) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 10, 16, 11, 21, 597, DateTimeKind.Utc).AddTicks(9072), "$2a$11$7CSbbEHUu7YUvSwv1vs8KeNVDB338eEEC5eHkLE5iysU3WmNa11Pi", new DateTime(2024, 8, 10, 16, 11, 21, 597, DateTimeKind.Utc).AddTicks(9079) });
        }
    }
}
