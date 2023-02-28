using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Nude.API.Migrations
{
    public partial class _0010 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParsingMetas_ParsingTickets_Id",
                table: "ParsingMetas");

            migrationBuilder.DropForeignKey(
                name: "FK_ParsingMetas_ParsingTickets_ParsingTicketId",
                table: "ParsingMetas");

            migrationBuilder.DropForeignKey(
                name: "FK_ParsingResults_ParsingTickets_Id",
                table: "ParsingResults");

            migrationBuilder.DropForeignKey(
                name: "FK_ParsingResults_ParsingTickets_ParsingTicketId",
                table: "ParsingResults");

            migrationBuilder.DropIndex(
                name: "IX_ParsingResults_ParsingTicketId",
                table: "ParsingResults");

            migrationBuilder.DropIndex(
                name: "IX_ParsingMetas_ParsingTicketId",
                table: "ParsingMetas");

            migrationBuilder.RenameColumn(
                name: "ParsingTicketId",
                table: "ParsingResults",
                newName: "TicketId");

            migrationBuilder.RenameColumn(
                name: "ParsingTicketId",
                table: "ParsingMetas",
                newName: "TicketId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ParsingResults",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ParsingMetas",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateIndex(
                name: "IX_ParsingResults_TicketId",
                table: "ParsingResults",
                column: "TicketId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParsingMetas_TicketId",
                table: "ParsingMetas",
                column: "TicketId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ParsingMetas_ParsingTickets_TicketId",
                table: "ParsingMetas",
                column: "TicketId",
                principalTable: "ParsingTickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParsingResults_ParsingTickets_TicketId",
                table: "ParsingResults",
                column: "TicketId",
                principalTable: "ParsingTickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParsingMetas_ParsingTickets_TicketId",
                table: "ParsingMetas");

            migrationBuilder.DropForeignKey(
                name: "FK_ParsingResults_ParsingTickets_TicketId",
                table: "ParsingResults");

            migrationBuilder.DropIndex(
                name: "IX_ParsingResults_TicketId",
                table: "ParsingResults");

            migrationBuilder.DropIndex(
                name: "IX_ParsingMetas_TicketId",
                table: "ParsingMetas");

            migrationBuilder.RenameColumn(
                name: "TicketId",
                table: "ParsingResults",
                newName: "ParsingTicketId");

            migrationBuilder.RenameColumn(
                name: "TicketId",
                table: "ParsingMetas",
                newName: "ParsingTicketId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ParsingResults",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ParsingMetas",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateIndex(
                name: "IX_ParsingResults_ParsingTicketId",
                table: "ParsingResults",
                column: "ParsingTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_ParsingMetas_ParsingTicketId",
                table: "ParsingMetas",
                column: "ParsingTicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParsingMetas_ParsingTickets_Id",
                table: "ParsingMetas",
                column: "Id",
                principalTable: "ParsingTickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParsingMetas_ParsingTickets_ParsingTicketId",
                table: "ParsingMetas",
                column: "ParsingTicketId",
                principalTable: "ParsingTickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParsingResults_ParsingTickets_Id",
                table: "ParsingResults",
                column: "Id",
                principalTable: "ParsingTickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParsingResults_ParsingTickets_ParsingTicketId",
                table: "ParsingResults",
                column: "ParsingTicketId",
                principalTable: "ParsingTickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
