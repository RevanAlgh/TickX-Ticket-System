using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticket.Data.Migrations
{
    /// <inheritdoc />
    public partial class m7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Comments",
                newName: "Replie");


            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 10, 16, 11, 21, 597, DateTimeKind.Utc).AddTicks(9072), "$2a$11$7CSbbEHUu7YUvSwv1vs8KeNVDB338eEEC5eHkLE5iysU3WmNa11Pi", new DateTime(2024, 8, 10, 16, 11, 21, 597, DateTimeKind.Utc).AddTicks(9079) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.RenameColumn(
                name: "Replie",
                table: "Comments",
                newName: "Text");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 10, 7, 23, 53, 136, DateTimeKind.Utc).AddTicks(5859), "$2a$11$SrIY3MBhonMhw2Elyef/me5jxBaxxjIeoL4.OEmJAZNx/K1jGEvEW", new DateTime(2024, 8, 10, 7, 23, 53, 136, DateTimeKind.Utc).AddTicks(5862) });
        }
    }
}
