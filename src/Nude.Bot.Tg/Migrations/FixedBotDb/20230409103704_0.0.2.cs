using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nude.Bot.Tg.Migrations.FixedBotDb
{
    public partial class _002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TicketId",
                table: "Messages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TicketType",
                table: "Messages",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "TicketType",
                table: "Messages");
        }
    }
}
