using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Nude.API.Migrations
{
    public partial class _003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormatTickets");

            migrationBuilder.DropTable(
                name: "ContentFormatTicketContext");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContentFormatTicketContext",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntityId = table.Column<string>(type: "text", nullable: false),
                    EntityType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentFormatTicketContext", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormatTickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContextId = table.Column<int>(type: "integer", nullable: false),
                    ResultId = table.Column<int>(type: "integer", nullable: true),
                    FormatType = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormatTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormatTickets_ContentFormatTicketContext_ContextId",
                        column: x => x.ContextId,
                        principalTable: "ContentFormatTicketContext",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormatTickets_FormattedContents_ResultId",
                        column: x => x.ResultId,
                        principalTable: "FormattedContents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormatTickets_ContextId",
                table: "FormatTickets",
                column: "ContextId");

            migrationBuilder.CreateIndex(
                name: "IX_FormatTickets_ResultId",
                table: "FormatTickets",
                column: "ResultId");
        }
    }
}
