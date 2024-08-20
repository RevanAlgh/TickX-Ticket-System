using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticket.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixingmanagerdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 7, 20, 42, 7, 264, DateTimeKind.Utc).AddTicks(3018), "$2a$11$WKpGQND0H8feN.1r4QoE1elSIaTdJ8Zdls9PCJNkNd0/l5RhboD8q", new DateTime(2024, 8, 7, 20, 42, 7, 264, DateTimeKind.Utc).AddTicks(3023) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 7, 18, 50, 17, 399, DateTimeKind.Utc).AddTicks(5589), "Password123", new DateTime(2024, 8, 7, 18, 50, 17, 399, DateTimeKind.Utc).AddTicks(5592) });
        }
    }
}
