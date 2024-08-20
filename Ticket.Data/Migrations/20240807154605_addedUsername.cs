using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticket.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedUsername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt", "UserName" },
                values: new object[] { new DateTime(2024, 8, 7, 15, 46, 5, 4, DateTimeKind.Utc).AddTicks(8809), new DateTime(2024, 8, 7, 15, 46, 5, 4, DateTimeKind.Utc).AddTicks(8814), "Manager" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 7, 8, 59, 18, 235, DateTimeKind.Utc).AddTicks(1004), new DateTime(2024, 8, 7, 8, 59, 18, 235, DateTimeKind.Utc).AddTicks(1008) });
        }
    }
}
