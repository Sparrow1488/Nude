using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nude.Bot.Tg.Migrations.FixedBotDb
{
    public partial class _003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserKey",
                table: "Messages",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserKey",
                table: "Messages");
        }
    }
}
