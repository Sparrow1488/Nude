using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Nude.API.Migrations
{
    public partial class _009 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParsingMetas_ParsingTickets_ParsingTicketId1",
                table: "ParsingMetas");

            migrationBuilder.DropForeignKey(
                name: "FK_ParsingResults_ParsingTickets_ParsingTicketId1",
                table: "ParsingResults");

            migrationBuilder.DropIndex(
                name: "IX_ParsingResults_ParsingTicketId",
                table: "ParsingResults");

            migrationBuilder.DropIndex(
                name: "IX_ParsingResults_ParsingTicketId1",
                table: "ParsingResults");

            migrationBuilder.DropIndex(
                name: "IX_ParsingMetas_ParsingTicketId",
                table: "ParsingMetas");

            migrationBuilder.DropIndex(
                name: "IX_ParsingMetas_ParsingTicketId1",
                table: "ParsingMetas");

            migrationBuilder.DropColumn(
                name: "ParsingTicketId1",
                table: "ParsingResults");

            migrationBuilder.DropColumn(
                name: "ParsingTicketId1",
                table: "ParsingMetas");

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
                name: "FK_ParsingResults_ParsingTickets_Id",
                table: "ParsingResults",
                column: "Id",
                principalTable: "ParsingTickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParsingMetas_ParsingTickets_Id",
                table: "ParsingMetas");

            migrationBuilder.DropForeignKey(
                name: "FK_ParsingResults_ParsingTickets_Id",
                table: "ParsingResults");

            migrationBuilder.DropIndex(
                name: "IX_ParsingResults_ParsingTicketId",
                table: "ParsingResults");

            migrationBuilder.DropIndex(
                name: "IX_ParsingMetas_ParsingTicketId",
                table: "ParsingMetas");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ParsingResults",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "ParsingTicketId1",
                table: "ParsingResults",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ParsingMetas",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "ParsingTicketId1",
                table: "ParsingMetas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ParsingResults_ParsingTicketId",
                table: "ParsingResults",
                column: "ParsingTicketId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParsingResults_ParsingTicketId1",
                table: "ParsingResults",
                column: "ParsingTicketId1");

            migrationBuilder.CreateIndex(
                name: "IX_ParsingMetas_ParsingTicketId",
                table: "ParsingMetas",
                column: "ParsingTicketId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParsingMetas_ParsingTicketId1",
                table: "ParsingMetas",
                column: "ParsingTicketId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ParsingMetas_ParsingTickets_ParsingTicketId1",
                table: "ParsingMetas",
                column: "ParsingTicketId1",
                principalTable: "ParsingTickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParsingResults_ParsingTickets_ParsingTicketId1",
                table: "ParsingResults",
                column: "ParsingTicketId1",
                principalTable: "ParsingTickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
