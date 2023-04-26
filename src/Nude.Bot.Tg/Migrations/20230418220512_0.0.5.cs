using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Nude.Bot.Tg.Migrations
{
    public partial class _005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "content_key",
                table: "messages");

            migrationBuilder.RenameColumn(
                name: "ticket_id",
                table: "messages",
                newName: "details_id");

            migrationBuilder.CreateTable(
                name: "message_details",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    discriminator = table.Column<string>(type: "text", nullable: false),
                    ticket_id = table.Column<int>(type: "integer", nullable: true),
                    content_key = table.Column<string>(type: "text", nullable: true),
                    media_group_id = table.Column<string>(type: "text", nullable: true),
                    current_media = table.Column<int>(type: "integer", nullable: true),
                    total_media = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_message_details", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_messages_details_id",
                table: "messages",
                column: "details_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_messages_message_details_details_id",
                table: "messages",
                column: "details_id",
                principalTable: "message_details",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_messages_message_details_details_id",
                table: "messages");

            migrationBuilder.DropTable(
                name: "message_details");

            migrationBuilder.DropIndex(
                name: "ix_messages_details_id",
                table: "messages");

            migrationBuilder.RenameColumn(
                name: "details_id",
                table: "messages",
                newName: "ticket_id");

            migrationBuilder.AddColumn<string>(
                name: "content_key",
                table: "messages",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
