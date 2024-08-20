using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticket.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedPriorities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 8, 19, 48, 0, 825, DateTimeKind.Utc).AddTicks(1177), "$2a$11$kZO/KDfmBjpEl.tQH3oKUuqWy0pzixnhZ4CbPvBKnvlkYaf73DAZy", new DateTime(2024, 8, 8, 19, 48, 0, 825, DateTimeKind.Utc).AddTicks(1182) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 8, 17, 9, 23, 990, DateTimeKind.Utc).AddTicks(113), "$2a$11$06ZWz6Ta5s0yHAuH/EJa0.bV9mf9MieuV0Qn4VLjp.C3jMpIAnxa.", new DateTime(2024, 8, 8, 17, 9, 23, 990, DateTimeKind.Utc).AddTicks(117) });
        }
    }
}
