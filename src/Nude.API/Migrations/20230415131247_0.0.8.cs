using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nude.API.Migrations
{
    public partial class _008 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "content_tickets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_content_tickets_user_id",
                table: "content_tickets",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_content_tickets_users_user_id",
                table: "content_tickets",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_content_tickets_users_user_id",
                table: "content_tickets");

            migrationBuilder.DropIndex(
                name: "ix_content_tickets_user_id",
                table: "content_tickets");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "content_tickets");
        }
    }
}
