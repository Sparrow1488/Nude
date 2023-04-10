using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Nude.API.Migrations
{
    public partial class _002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentTickets_ContentResults_ResultId",
                table: "ContentTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentTickets_TicketContexts_ContextId",
                table: "ContentTickets");

            migrationBuilder.DropTable(
                name: "ContentResults");

            migrationBuilder.DropTable(
                name: "TicketContexts");

            migrationBuilder.DropIndex(
                name: "IX_ContentTickets_ContextId",
                table: "ContentTickets");

            migrationBuilder.DropIndex(
                name: "IX_ContentTickets_ResultId",
                table: "ContentTickets");

            migrationBuilder.DropColumn(
                name: "ContextId",
                table: "ContentTickets");

            migrationBuilder.DropColumn(
                name: "ResultId",
                table: "ContentTickets");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ContentTickets");

            migrationBuilder.AddColumn<string>(
                name: "ContentKey",
                table: "Mangas",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContentKey",
                table: "ContentTickets",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContentUrl",
                table: "ContentTickets",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentKey",
                table: "Mangas");

            migrationBuilder.DropColumn(
                name: "ContentKey",
                table: "ContentTickets");

            migrationBuilder.DropColumn(
                name: "ContentUrl",
                table: "ContentTickets");

            migrationBuilder.AddColumn<int>(
                name: "ContextId",
                table: "ContentTickets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResultId",
                table: "ContentTickets",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ContentTickets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ContentResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    EntityId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketContexts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContentId = table.Column<string>(type: "text", nullable: true),
                    ContentUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketContexts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentTickets_ContextId",
                table: "ContentTickets",
                column: "ContextId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTickets_ResultId",
                table: "ContentTickets",
                column: "ResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTickets_ContentResults_ResultId",
                table: "ContentTickets",
                column: "ResultId",
                principalTable: "ContentResults",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTickets_TicketContexts_ContextId",
                table: "ContentTickets",
                column: "ContextId",
                principalTable: "TicketContexts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
