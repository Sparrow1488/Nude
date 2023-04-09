using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Nude.API.Migrations
{
    public partial class _001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContentFormatTicketContext",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntityId = table.Column<string>(type: "text", nullable: false),
                    EntityType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentFormatTicketContext", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContentResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntityId = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentResults", x => x.Id);
                });

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
                name: "Servers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NotificationsCallbackUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(type: "text", nullable: false),
                    NormalizeValue = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketContexts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContentUrl = table.Column<string>(type: "text", nullable: false),
                    ContentId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketContexts", x => x.Id);
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
                name: "ContentTickets",
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
                    table.PrimaryKey("PK_ContentTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentTickets_ContentResults_ResultId",
                        column: x => x.ResultId,
                        principalTable: "ContentResults",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContentTickets_TicketContexts_ContextId",
                        column: x => x.ContextId,
                        principalTable: "TicketContexts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormattedContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    MangaEntryId = table.Column<int>(type: "integer", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormattedContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormattedContents_Mangas_MangaEntryId",
                        column: x => x.MangaEntryId,
                        principalTable: "Mangas",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateTable(
                name: "MangaImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UrlId = table.Column<int>(type: "integer", nullable: false),
                    MangaEntryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MangaImages_Mangas_MangaEntryId",
                        column: x => x.MangaEntryId,
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
                name: "FormatTickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FormatType = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ResultId = table.Column<int>(type: "integer", nullable: true),
                    ContextId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormatTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormatTickets_ContentFormatTicketContext_ContextId",
                        column: x => x.ContextId,
                        principalTable: "ContentFormatTicketContext",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormatTickets_FormattedContents_ResultId",
                        column: x => x.ResultId,
                        principalTable: "FormattedContents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentTickets_ContextId",
                table: "ContentTickets",
                column: "ContextId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTickets_ResultId",
                table: "ContentTickets",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_FormattedContents_MangaEntryId",
                table: "FormattedContents",
                column: "MangaEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_FormatTickets_ContextId",
                table: "FormatTickets",
                column: "ContextId");

            migrationBuilder.CreateIndex(
                name: "IX_FormatTickets_ResultId",
                table: "FormatTickets",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaEntryTag_TagsId",
                table: "MangaEntryTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaImages_MangaEntryId",
                table: "MangaImages",
                column: "MangaEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaImages_UrlId",
                table: "MangaImages",
                column: "UrlId");

            migrationBuilder.CreateIndex(
                name: "IX_Mangas_ExternalMetaId",
                table: "Mangas",
                column: "ExternalMetaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentTickets");

            migrationBuilder.DropTable(
                name: "FormatTickets");

            migrationBuilder.DropTable(
                name: "MangaEntryTag");

            migrationBuilder.DropTable(
                name: "MangaImages");

            migrationBuilder.DropTable(
                name: "Servers");

            migrationBuilder.DropTable(
                name: "ContentResults");

            migrationBuilder.DropTable(
                name: "TicketContexts");

            migrationBuilder.DropTable(
                name: "ContentFormatTicketContext");

            migrationBuilder.DropTable(
                name: "FormattedContents");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Urls");

            migrationBuilder.DropTable(
                name: "Mangas");

            migrationBuilder.DropTable(
                name: "MangaExternalMetas");
        }
    }
}
