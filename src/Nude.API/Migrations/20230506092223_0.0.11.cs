using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Nude.API.Migrations
{
    public partial class _0011 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "collection_image");

            migrationBuilder.CreateTable(
                name: "collection_image_entry",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    entry_id = table.Column<int>(type: "integer", nullable: false),
                    collection_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_collection_image_entry", x => x.id);
                    table.ForeignKey(
                        name: "fk_collection_image_entry_image_collections_collection_id",
                        column: x => x.collection_id,
                        principalTable: "image_collections",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_collection_image_entry_images_entry_id",
                        column: x => x.entry_id,
                        principalTable: "images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "view",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    manga_id = table.Column<int>(type: "integer", nullable: true),
                    image_id = table.Column<int>(type: "integer", nullable: true),
                    image_collection_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_view", x => x.id);
                    table.ForeignKey(
                        name: "fk_view_image_collections_image_collection_id",
                        column: x => x.image_collection_id,
                        principalTable: "image_collections",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_view_images_image_id",
                        column: x => x.image_id,
                        principalTable: "images",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_view_mangas_manga_id",
                        column: x => x.manga_id,
                        principalTable: "mangas",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_view_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_collection_image_entry_collection_id",
                table: "collection_image_entry",
                column: "collection_id");

            migrationBuilder.CreateIndex(
                name: "ix_collection_image_entry_entry_id",
                table: "collection_image_entry",
                column: "entry_id");

            migrationBuilder.CreateIndex(
                name: "ix_view_image_collection_id",
                table: "view",
                column: "image_collection_id");

            migrationBuilder.CreateIndex(
                name: "ix_view_image_id",
                table: "view",
                column: "image_id");

            migrationBuilder.CreateIndex(
                name: "ix_view_manga_id",
                table: "view",
                column: "manga_id");

            migrationBuilder.CreateIndex(
                name: "ix_view_user_id",
                table: "view",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "collection_image_entry");

            migrationBuilder.DropTable(
                name: "view");

            migrationBuilder.CreateTable(
                name: "collection_image",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    collection_id = table.Column<int>(type: "integer", nullable: false),
                    entry_id = table.Column<int>(type: "integer", nullable: false)
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
                name: "ix_collection_image_collection_id",
                table: "collection_image",
                column: "collection_id");

            migrationBuilder.CreateIndex(
                name: "ix_collection_image_entry_id",
                table: "collection_image",
                column: "entry_id");
        }
    }
}
