using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticket.Data.Migrations
{
    /// <inheritdoc />
    public partial class modifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Attachments_AttachmentId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Comments_CommentId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Comments_CommentId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CommentId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_AttachmentId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_CommentId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AttachmentId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Attachments");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TicketId",
                table: "Comments",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_TicketId",
                table: "Attachments",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Tickets_TicketId",
                table: "Attachments",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Tickets_TicketId",
                table: "Comments",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Tickets_TicketId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Tickets_TicketId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_TicketId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_TicketId",
                table: "Attachments");

            migrationBuilder.AddColumn<int>(
                name: "CommentId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AttachmentId",
                table: "Tickets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CommentId",
                table: "Tickets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Attachments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CommentId",
                table: "Users",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_AttachmentId",
                table: "Tickets",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CommentId",
                table: "Tickets",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Attachments_AttachmentId",
                table: "Tickets",
                column: "AttachmentId",
                principalTable: "Attachments",
                principalColumn: "AttachmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Comments_CommentId",
                table: "Tickets",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Comments_CommentId",
                table: "Users",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "CommentId");
        }
    }
}
