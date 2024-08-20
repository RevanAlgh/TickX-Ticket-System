using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticket.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedDTOs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 5, 10, 28, 12, 242, DateTimeKind.Utc).AddTicks(2904), new DateTime(2024, 8, 5, 10, 28, 12, 242, DateTimeKind.Utc).AddTicks(2907) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 5, 8, 26, 0, 90, DateTimeKind.Utc).AddTicks(2492), new DateTime(2024, 8, 5, 8, 26, 0, 90, DateTimeKind.Utc).AddTicks(2494) });
        }
    }
}
