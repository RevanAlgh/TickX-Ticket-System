using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticket.Data.Migrations
{
    /// <inheritdoc />
    public partial class seedingdatamanager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Address", "CreatedAt", "DOB", "Email", "FullName", "IsActive", "MobileNumber", "Password", "Role", "Token", "UpdatedAt", "UserImage" },
                values: new object[] { 1, "123 St, Riyadh, KSA", new DateTime(2024, 8, 4, 22, 8, 31, 349, DateTimeKind.Utc).AddTicks(6546), new DateOnly(2000, 1, 11), "Manager@gmail.com", "Manager", true, "123-456-7890", "Password123", 3, "", new DateTime(2024, 8, 4, 22, 8, 31, 349, DateTimeKind.Utc).AddTicks(6551), "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);
        }
    }
}
