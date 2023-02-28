using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Nude.API.Migrations
{
    public partial class _008 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "ParsingTickets");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "ParsingTickets");

            migrationBuilder.DropColumn(
                name: "UniqueId",
                table: "ParsingTickets");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "ParsingTickets");

            migrationBuilder.CreateTable(
                name: "FeedBackInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CallbackUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedBackInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParsingMetas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SourceItemId = table.Column<string>(type: "text", nullable: true),
                    SourceUrl = table.Column<string>(type: "text", nullable: false),
                    EntityType = table.Column<int>(type: "integer", nullable: false),
                    ParsingTicketId1 = table.Column<int>(type: "integer", nullable: false),
                    ParsingTicketId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParsingMetas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParsingMetas_ParsingTickets_ParsingTicketId",
                        column: x => x.ParsingTicketId,
                        principalTable: "ParsingTickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParsingMetas_ParsingTickets_ParsingTicketId1",
                        column: x => x.ParsingTicketId1,
                        principalTable: "ParsingTickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParsingResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Message = table.Column<string>(type: "text", nullable: false),
                    EntityId = table.Column<string>(type: "text", nullable: true),
                    StatusCode = table.Column<string>(type: "text", nullable: false),
                    ParsingTicketId1 = table.Column<int>(type: "integer", nullable: false),
                    ParsingTicketId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParsingResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParsingResults_ParsingTickets_ParsingTicketId",
                        column: x => x.ParsingTicketId,
                        principalTable: "ParsingTickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParsingResults_ParsingTickets_ParsingTicketId1",
                        column: x => x.ParsingTicketId1,
                        principalTable: "ParsingTickets",
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
                    FeedBackInfoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscribers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscribers_FeedBackInfos_FeedBackInfoId",
                        column: x => x.FeedBackInfoId,
                        principalTable: "FeedBackInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParsingTicketSubscriber",
                columns: table => new
                {
                    SubscribersId = table.Column<int>(type: "integer", nullable: false),
                    TicketsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParsingTicketSubscriber", x => new { x.SubscribersId, x.TicketsId });
                    table.ForeignKey(
                        name: "FK_ParsingTicketSubscriber_ParsingTickets_TicketsId",
                        column: x => x.TicketsId,
                        principalTable: "ParsingTickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParsingTicketSubscriber_Subscribers_SubscribersId",
                        column: x => x.SubscribersId,
                        principalTable: "Subscribers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParsingMetas_ParsingTicketId",
                table: "ParsingMetas",
                column: "ParsingTicketId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParsingMetas_ParsingTicketId1",
                table: "ParsingMetas",
                column: "ParsingTicketId1");

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
                name: "IX_ParsingTicketSubscriber_TicketsId",
                table: "ParsingTicketSubscriber",
                column: "TicketsId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscribers_FeedBackInfoId",
                table: "Subscribers",
                column: "FeedBackInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParsingMetas");

            migrationBuilder.DropTable(
                name: "ParsingResults");

            migrationBuilder.DropTable(
                name: "ParsingTicketSubscriber");

            migrationBuilder.DropTable(
                name: "Subscribers");

            migrationBuilder.DropTable(
                name: "FeedBackInfos");

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "ParsingTickets",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "ParsingTickets",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UniqueId",
                table: "ParsingTickets",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "ParsingTickets",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
