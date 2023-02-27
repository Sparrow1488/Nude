using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nude.API.Migrations
{
    public partial class _005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Mangas_MangaId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_MangaId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "MangaId",
                table: "Tags");

            migrationBuilder.CreateTable(
                name: "MangaTag",
                columns: table => new
                {
                    MangasId = table.Column<int>(type: "integer", nullable: false),
                    TagsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaTag", x => new { x.MangasId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_MangaTag_Mangas_MangasId",
                        column: x => x.MangasId,
                        principalTable: "Mangas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangaTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MangaTag_TagsId",
                table: "MangaTag",
                column: "TagsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MangaTag");

            migrationBuilder.AddColumn<int>(
                name: "MangaId",
                table: "Tags",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_MangaId",
                table: "Tags",
                column: "MangaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Mangas_MangaId",
                table: "Tags",
                column: "MangaId",
                principalTable: "Mangas",
                principalColumn: "Id");
        }
    }
}
