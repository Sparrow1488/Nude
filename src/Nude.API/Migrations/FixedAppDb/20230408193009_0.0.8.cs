using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Nude.API.Migrations.FixedAppDb
{
    public partial class _008 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentRequests_ContentResults_ResultId",
                table: "ContentRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentRequests_TicketContexts_ContextId",
                table: "ContentRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscribers_ContentRequests_ContentTicketId",
                table: "Subscribers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContentRequests",
                table: "ContentRequests");

            migrationBuilder.RenameTable(
                name: "ContentRequests",
                newName: "ContentTickets");

            migrationBuilder.RenameIndex(
                name: "IX_ContentRequests_ResultId",
                table: "ContentTickets",
                newName: "IX_ContentTickets_ResultId");

            migrationBuilder.RenameIndex(
                name: "IX_ContentRequests_ContextId",
                table: "ContentTickets",
                newName: "IX_ContentTickets_ContextId");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizeValue",
                table: "Tags",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "ContextId",
                table: "FormatTickets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContentTickets",
                table: "ContentTickets",
                column: "Id");

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

            migrationBuilder.CreateIndex(
                name: "IX_FormatTickets_ContextId",
                table: "FormatTickets",
                column: "ContextId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_FormatTickets_ContentFormatTicketContext_ContextId",
                table: "FormatTickets",
                column: "ContextId",
                principalTable: "ContentFormatTicketContext",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribers_ContentTickets_ContentTicketId",
                table: "Subscribers",
                column: "ContentTicketId",
                principalTable: "ContentTickets",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentTickets_ContentResults_ResultId",
                table: "ContentTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentTickets_TicketContexts_ContextId",
                table: "ContentTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_FormatTickets_ContentFormatTicketContext_ContextId",
                table: "FormatTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscribers_ContentTickets_ContentTicketId",
                table: "Subscribers");

            migrationBuilder.DropTable(
                name: "ContentFormatTicketContext");

            migrationBuilder.DropIndex(
                name: "IX_FormatTickets_ContextId",
                table: "FormatTickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContentTickets",
                table: "ContentTickets");

            migrationBuilder.DropColumn(
                name: "ContextId",
                table: "FormatTickets");

            migrationBuilder.RenameTable(
                name: "ContentTickets",
                newName: "ContentRequests");

            migrationBuilder.RenameIndex(
                name: "IX_ContentTickets_ResultId",
                table: "ContentRequests",
                newName: "IX_ContentRequests_ResultId");

            migrationBuilder.RenameIndex(
                name: "IX_ContentTickets_ContextId",
                table: "ContentRequests",
                newName: "IX_ContentRequests_ContextId");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizeValue",
                table: "Tags",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContentRequests",
                table: "ContentRequests",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentRequests_ContentResults_ResultId",
                table: "ContentRequests",
                column: "ResultId",
                principalTable: "ContentResults",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentRequests_TicketContexts_ContextId",
                table: "ContentRequests",
                column: "ContextId",
                principalTable: "TicketContexts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribers_ContentRequests_ContentTicketId",
                table: "Subscribers",
                column: "ContentTicketId",
                principalTable: "ContentRequests",
                principalColumn: "Id");
        }
    }
}
