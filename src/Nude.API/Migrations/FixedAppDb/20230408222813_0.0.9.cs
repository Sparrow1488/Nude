using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nude.API.Migrations.FixedAppDb
{
    public partial class _009 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscribers_ContentTickets_ContentTicketId",
                table: "Subscribers");

            migrationBuilder.DropIndex(
                name: "IX_Subscribers_ContentTicketId",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "ContentTicketId",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "NotifyStatus",
                table: "Subscribers");

            migrationBuilder.AddColumn<string>(
                name: "EntityId",
                table: "Subscribers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EntityType",
                table: "Subscribers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "Subscribers");

            migrationBuilder.AddColumn<int>(
                name: "ContentTicketId",
                table: "Subscribers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NotifyStatus",
                table: "Subscribers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Subscribers_ContentTicketId",
                table: "Subscribers",
                column: "ContentTicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribers_ContentTickets_ContentTicketId",
                table: "Subscribers",
                column: "ContentTicketId",
                principalTable: "ContentTickets",
                principalColumn: "Id");
        }
    }
}
