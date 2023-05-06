using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Nude.API.Migrations
{
    public partial class _0013 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "blacklist_id",
                table: "users",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "blacklists",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_blacklists", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "blacklist_tag",
                columns: table => new
                {
                    blacklists_id = table.Column<int>(type: "integer", nullable: false),
                    tags_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_blacklist_tag", x => new { x.blacklists_id, x.tags_id });
                    table.ForeignKey(
                        name: "fk_blacklist_tag_blacklists_blacklists_id",
                        column: x => x.blacklists_id,
                        principalTable: "blacklists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_blacklist_tag_tags_tags_id",
                        column: x => x.tags_id,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_users_blacklist_id",
                table: "users",
                column: "blacklist_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_blacklist_tag_tags_id",
                table: "blacklist_tag",
                column: "tags_id");

            migrationBuilder.AddForeignKey(
                name: "fk_users_blacklists_blacklist_id",
                table: "users",
                column: "blacklist_id",
                principalTable: "blacklists",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_users_blacklists_blacklist_id",
                table: "users");

            migrationBuilder.DropTable(
                name: "blacklist_tag");

            migrationBuilder.DropTable(
                name: "blacklists");

            migrationBuilder.DropIndex(
                name: "ix_users_blacklist_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "blacklist_id",
                table: "users");
        }
    }
}
