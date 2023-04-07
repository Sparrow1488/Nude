using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nude.API.Migrations.FixedAppDb
{
    public partial class _004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormattedContents_Mangas_MangaId",
                table: "FormattedContents");

            migrationBuilder.DropForeignKey(
                name: "FK_MangaImages_Mangas_MangaId",
                table: "MangaImages");

            migrationBuilder.DropTable(
                name: "MangaTag");

            migrationBuilder.RenameColumn(
                name: "MangaId",
                table: "MangaImages",
                newName: "MangaEntryId");

            migrationBuilder.RenameIndex(
                name: "IX_MangaImages_MangaId",
                table: "MangaImages",
                newName: "IX_MangaImages_MangaEntryId");

            migrationBuilder.RenameColumn(
                name: "MangaId",
                table: "FormattedContents",
                newName: "MangaEntryId");

            migrationBuilder.RenameIndex(
                name: "IX_FormattedContents_MangaId",
                table: "FormattedContents",
                newName: "IX_FormattedContents_MangaEntryId");

            migrationBuilder.CreateTable(
                name: "MangaEntryTag",
                columns: table => new
                {
                    MangasId = table.Column<int>(type: "integer", nullable: false),
                    TagsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaEntryTag", x => new { x.MangasId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_MangaEntryTag_Mangas_MangasId",
                        column: x => x.MangasId,
                        principalTable: "Mangas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangaEntryTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MangaEntryTag_TagsId",
                table: "MangaEntryTag",
                column: "TagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormattedContents_Mangas_MangaEntryId",
                table: "FormattedContents",
                column: "MangaEntryId",
                principalTable: "Mangas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MangaImages_Mangas_MangaEntryId",
                table: "MangaImages",
                column: "MangaEntryId",
                principalTable: "Mangas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormattedContents_Mangas_MangaEntryId",
                table: "FormattedContents");

            migrationBuilder.DropForeignKey(
                name: "FK_MangaImages_Mangas_MangaEntryId",
                table: "MangaImages");

            migrationBuilder.DropTable(
                name: "MangaEntryTag");

            migrationBuilder.RenameColumn(
                name: "MangaEntryId",
                table: "MangaImages",
                newName: "MangaId");

            migrationBuilder.RenameIndex(
                name: "IX_MangaImages_MangaEntryId",
                table: "MangaImages",
                newName: "IX_MangaImages_MangaId");

            migrationBuilder.RenameColumn(
                name: "MangaEntryId",
                table: "FormattedContents",
                newName: "MangaId");

            migrationBuilder.RenameIndex(
                name: "IX_FormattedContents_MangaEntryId",
                table: "FormattedContents",
                newName: "IX_FormattedContents_MangaId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_FormattedContents_Mangas_MangaId",
                table: "FormattedContents",
                column: "MangaId",
                principalTable: "Mangas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MangaImages_Mangas_MangaId",
                table: "MangaImages",
                column: "MangaId",
                principalTable: "Mangas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
