using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticket.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixingmanagerdata1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 7, 20, 45, 19, 102, DateTimeKind.Utc).AddTicks(3489), "$2a$11$LGLXpgkewT/9mV8quYfno.ZM/gXulAn2xF3pt6U.I1MnE9s5WsYiO", new DateTime(2024, 8, 7, 20, 45, 19, 102, DateTimeKind.Utc).AddTicks(3495) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 7, 20, 42, 7, 264, DateTimeKind.Utc).AddTicks(3018), "$2a$11$WKpGQND0H8feN.1r4QoE1elSIaTdJ8Zdls9PCJNkNd0/l5RhboD8q", new DateTime(2024, 8, 7, 20, 42, 7, 264, DateTimeKind.Utc).AddTicks(3023) });
        }
    }
}
