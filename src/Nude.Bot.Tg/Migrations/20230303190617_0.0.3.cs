using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nude.Bot.Tg.Migrations
{
    public partial class _003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParsingTicketId",
                table: "ConvertingTickets");

            migrationBuilder.AddColumn<int>(
                name: "ParsingId",
                table: "ConvertingTickets",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParsingId",
                table: "ConvertingTickets");

            migrationBuilder.AddColumn<string>(
                name: "ParsingTicketId",
                table: "ConvertingTickets",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
