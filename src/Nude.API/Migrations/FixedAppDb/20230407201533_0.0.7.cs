using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Nude.API.Migrations.FixedAppDb
{
    public partial class _007 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormattingRequests_FormattedContents_ResultId",
                table: "FormattingRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiveRequests_ReceiveContexts_ContextId",
                table: "ReceiveRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiveRequests_ReceiveResults_ResultId",
                table: "ReceiveRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscribers_ReceiveRequests_ContentTicketId",
                table: "Subscribers");

            migrationBuilder.DropTable(
                name: "ReceiveContexts");

            migrationBuilder.DropTable(
                name: "ReceiveResults");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReceiveRequests",
                table: "ReceiveRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormattingRequests",
                table: "FormattingRequests");

            migrationBuilder.RenameTable(
                name: "ReceiveRequests",
                newName: "ContentRequests");

            migrationBuilder.RenameTable(
                name: "FormattingRequests",
                newName: "FormatTickets");

            migrationBuilder.RenameIndex(
                name: "IX_ReceiveRequests_ResultId",
                table: "ContentRequests",
                newName: "IX_ContentRequests_ResultId");

            migrationBuilder.RenameIndex(
                name: "IX_ReceiveRequests_ContextId",
                table: "ContentRequests",
                newName: "IX_ContentRequests_ContextId");

            migrationBuilder.RenameIndex(
                name: "IX_FormattingRequests_ResultId",
                table: "FormatTickets",
                newName: "IX_FormatTickets_ResultId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContentRequests",
                table: "ContentRequests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormatTickets",
                table: "FormatTickets",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ContentResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntityId = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false)
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
                    ContentUrl = table.Column<string>(type: "text", nullable: false),
                    ContentId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketContexts", x => x.Id);
                });

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
                name: "FK_FormatTickets_FormattedContents_ResultId",
                table: "FormatTickets",
                column: "ResultId",
                principalTable: "FormattedContents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribers_ContentRequests_ContentTicketId",
                table: "Subscribers",
                column: "ContentTicketId",
                principalTable: "ContentRequests",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentRequests_ContentResults_ResultId",
                table: "ContentRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentRequests_TicketContexts_ContextId",
                table: "ContentRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_FormatTickets_FormattedContents_ResultId",
                table: "FormatTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscribers_ContentRequests_ContentTicketId",
                table: "Subscribers");

            migrationBuilder.DropTable(
                name: "ContentResults");

            migrationBuilder.DropTable(
                name: "TicketContexts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormatTickets",
                table: "FormatTickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContentRequests",
                table: "ContentRequests");

            migrationBuilder.RenameTable(
                name: "FormatTickets",
                newName: "FormattingRequests");

            migrationBuilder.RenameTable(
                name: "ContentRequests",
                newName: "ReceiveRequests");

            migrationBuilder.RenameIndex(
                name: "IX_FormatTickets_ResultId",
                table: "FormattingRequests",
                newName: "IX_FormattingRequests_ResultId");

            migrationBuilder.RenameIndex(
                name: "IX_ContentRequests_ResultId",
                table: "ReceiveRequests",
                newName: "IX_ReceiveRequests_ResultId");

            migrationBuilder.RenameIndex(
                name: "IX_ContentRequests_ContextId",
                table: "ReceiveRequests",
                newName: "IX_ReceiveRequests_ContextId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormattingRequests",
                table: "FormattingRequests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReceiveRequests",
                table: "ReceiveRequests",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ReceiveContexts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContentId = table.Column<string>(type: "text", nullable: true),
                    ContentUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiveContexts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReceiveResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    EntityId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiveResults", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_FormattingRequests_FormattedContents_ResultId",
                table: "FormattingRequests",
                column: "ResultId",
                principalTable: "FormattedContents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiveRequests_ReceiveContexts_ContextId",
                table: "ReceiveRequests",
                column: "ContextId",
                principalTable: "ReceiveContexts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiveRequests_ReceiveResults_ResultId",
                table: "ReceiveRequests",
                column: "ResultId",
                principalTable: "ReceiveResults",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribers_ReceiveRequests_ContentTicketId",
                table: "Subscribers",
                column: "ContentTicketId",
                principalTable: "ReceiveRequests",
                principalColumn: "Id");
        }
    }
}
