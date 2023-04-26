using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Nude.API.Migrations
{
    public partial class _005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "image_collection_id",
                table: "formats",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "image_collections",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    content_key = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_image_collections", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "collection_image",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    entry_id = table.Column<int>(type: "integer", nullable: false),
                    collection_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_collection_image", x => x.id);
                    table.ForeignKey(
                        name: "fk_collection_image_image_collections_collection_id",
                        column: x => x.collection_id,
                        principalTable: "image_collections",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_collection_image_images_entry_id",
                        column: x => x.entry_id,
                        principalTable: "images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_formats_image_collection_id",
                table: "formats",
                column: "image_collection_id");

            migrationBuilder.CreateIndex(
                name: "ix_collection_image_collection_id",
                table: "collection_image",
                column: "collection_id");

            migrationBuilder.CreateIndex(
                name: "ix_collection_image_entry_id",
                table: "collection_image",
                column: "entry_id");

            migrationBuilder.AddForeignKey(
                name: "fk_formats_image_collections_image_collection_id",
                table: "formats",
                column: "image_collection_id",
                principalTable: "image_collections",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_formats_image_collections_image_collection_id",
                table: "formats");

            migrationBuilder.DropTable(
                name: "collection_image");

            migrationBuilder.DropTable(
                name: "image_collections");

            migrationBuilder.DropIndex(
                name: "ix_formats_image_collection_id",
                table: "formats");

            migrationBuilder.DropColumn(
                name: "image_collection_id",
                table: "formats");
        }
    }
}
