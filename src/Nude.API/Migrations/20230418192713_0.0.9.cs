using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nude.API.Migrations
{
    public partial class _009 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "owner_id",
                table: "images",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_images_owner_id",
                table: "images",
                column: "owner_id");

            migrationBuilder.AddForeignKey(
                name: "fk_images_users_owner_id",
                table: "images",
                column: "owner_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_images_users_owner_id",
                table: "images");

            migrationBuilder.DropIndex(
                name: "ix_images_owner_id",
                table: "images");

            migrationBuilder.DropColumn(
                name: "owner_id",
                table: "images");
        }
    }
}
