using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Nude.API.Migrations.FixedAppDb
{
    public partial class _002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mangas_MangaExternalMetas_ExternalMetaId",
                table: "Mangas");

            migrationBuilder.DropTable(
                name: "MangaExternalMetas");

            migrationBuilder.CreateTable(
                name: "ExternalMangaMetas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SourceId = table.Column<string>(type: "text", nullable: true),
                    SourceUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalMangaMetas", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Mangas_ExternalMangaMetas_ExternalMetaId",
                table: "Mangas",
                column: "ExternalMetaId",
                principalTable: "ExternalMangaMetas",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mangas_ExternalMangaMetas_ExternalMetaId",
                table: "Mangas");

            migrationBuilder.DropTable(
                name: "ExternalMangaMetas");

            migrationBuilder.CreateTable(
                name: "MangaExternalMetas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SourceId = table.Column<string>(type: "text", nullable: true),
                    SourceUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaExternalMetas", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Mangas_MangaExternalMetas_ExternalMetaId",
                table: "Mangas",
                column: "ExternalMetaId",
                principalTable: "MangaExternalMetas",
                principalColumn: "Id");
        }
    }
}
