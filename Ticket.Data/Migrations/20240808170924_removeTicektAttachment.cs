using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticket.Data.Migrations
{
    /// <inheritdoc />
    public partial class removeTicektAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attachment",
                table: "Tickets");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 8, 17, 9, 23, 990, DateTimeKind.Utc).AddTicks(113), "$2a$11$06ZWz6Ta5s0yHAuH/EJa0.bV9mf9MieuV0Qn4VLjp.C3jMpIAnxa.", new DateTime(2024, 8, 8, 17, 9, 23, 990, DateTimeKind.Utc).AddTicks(117) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Attachment",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 7, 20, 45, 19, 102, DateTimeKind.Utc).AddTicks(3489), "$2a$11$LGLXpgkewT/9mV8quYfno.ZM/gXulAn2xF3pt6U.I1MnE9s5WsYiO", new DateTime(2024, 8, 7, 20, 45, 19, 102, DateTimeKind.Utc).AddTicks(3495) });
        }
    }
}
