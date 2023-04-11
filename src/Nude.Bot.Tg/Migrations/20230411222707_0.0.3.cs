using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nude.Bot.Tg.Migrations
{
    public partial class _003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "TicketType",
                table: "Messages");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "messages");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "messages",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "messages",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "TicketId",
                table: "messages",
                newName: "ticket_id");

            migrationBuilder.RenameColumn(
                name: "MessageId",
                table: "messages",
                newName: "message_id");

            migrationBuilder.RenameColumn(
                name: "ContentKey",
                table: "messages",
                newName: "content_key");

            migrationBuilder.RenameColumn(
                name: "ChatId",
                table: "messages",
                newName: "chat_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_messages",
                table: "messages",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_messages",
                table: "messages");

            migrationBuilder.RenameTable(
                name: "messages",
                newName: "Messages");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Messages",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Messages",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ticket_id",
                table: "Messages",
                newName: "TicketId");

            migrationBuilder.RenameColumn(
                name: "message_id",
                table: "Messages",
                newName: "MessageId");

            migrationBuilder.RenameColumn(
                name: "content_key",
                table: "Messages",
                newName: "ContentKey");

            migrationBuilder.RenameColumn(
                name: "chat_id",
                table: "Messages",
                newName: "ChatId");

            migrationBuilder.AddColumn<string>(
                name: "TicketType",
                table: "Messages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "Id");
        }
    }
}
