using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Nude.API.Migrations.FixedAppDb
{
    public partial class _001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "ReceiveContexts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContentUrl = table.Column<string>(type: "text", nullable: false),
                    ContentId = table.Column<string>(type: "text", nullable: true)
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
                    EntityId = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiveResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(type: "text", nullable: false),
                    NormalizeValue = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Urls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Urls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mangas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ExternalMetaId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mangas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mangas_MangaExternalMetas_ExternalMetaId",
                        column: x => x.ExternalMetaId,
                        principalTable: "MangaExternalMetas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReceiveRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ResultId = table.Column<int>(type: "integer", nullable: true),
                    ContextId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiveRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiveRequests_ReceiveContexts_ContextId",
                        column: x => x.ContextId,
                        principalTable: "ReceiveContexts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiveRequests_ReceiveResults_ResultId",
                        column: x => x.ResultId,
                        principalTable: "ReceiveResults",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FormattedContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    MangaId = table.Column<int>(type: "integer", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormattedContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormattedContents_Mangas_MangaId",
                        column: x => x.MangaId,
                        principalTable: "Mangas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MangaImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UrlId = table.Column<int>(type: "integer", nullable: false),
                    MangaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MangaImages_Mangas_MangaId",
                        column: x => x.MangaId,
                        principalTable: "Mangas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangaImages_Urls_UrlId",
                        column: x => x.UrlId,
                        principalTable: "Urls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "Subscribers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NotifyStatus = table.Column<int>(type: "integer", nullable: false),
                    CallbackUrl = table.Column<string>(type: "text", nullable: true),
                    ReceiveContentRequestId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscribers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscribers_ReceiveRequests_ReceiveContentRequestId",
                        column: x => x.ReceiveContentRequestId,
                        principalTable: "ReceiveRequests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FormattingRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FormatType = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ResultId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormattingRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormattingRequests_FormattedContents_ResultId",
                        column: x => x.ResultId,
                        principalTable: "FormattedContents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormattedContents_MangaId",
                table: "FormattedContents",
                column: "MangaId");

            migrationBuilder.CreateIndex(
                name: "IX_FormattingRequests_ResultId",
                table: "FormattingRequests",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaImages_MangaId",
                table: "MangaImages",
                column: "MangaId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaImages_UrlId",
                table: "MangaImages",
                column: "UrlId");

            migrationBuilder.CreateIndex(
                name: "IX_Mangas_ExternalMetaId",
                table: "Mangas",
                column: "ExternalMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaTag_TagsId",
                table: "MangaTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveRequests_ContextId",
                table: "ReceiveRequests",
                column: "ContextId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveRequests_ResultId",
                table: "ReceiveRequests",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscribers_ReceiveContentRequestId",
                table: "Subscribers",
                column: "ReceiveContentRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormattingRequests");

            migrationBuilder.DropTable(
                name: "MangaImages");

            migrationBuilder.DropTable(
                name: "MangaTag");

            migrationBuilder.DropTable(
                name: "Subscribers");

            migrationBuilder.DropTable(
                name: "FormattedContents");

            migrationBuilder.DropTable(
                name: "Urls");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "ReceiveRequests");

            migrationBuilder.DropTable(
                name: "Mangas");

            migrationBuilder.DropTable(
                name: "ReceiveContexts");

            migrationBuilder.DropTable(
                name: "ReceiveResults");

            migrationBuilder.DropTable(
                name: "MangaExternalMetas");
        }
    }
}
