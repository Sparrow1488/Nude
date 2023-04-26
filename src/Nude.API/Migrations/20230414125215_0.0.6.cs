using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nude.API.Migrations
{
    public partial class _006 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_formats_image_collections_image_collection_id",
                table: "formats");

            migrationBuilder.AlterColumn<int>(
                name: "manga_entry_id",
                table: "formats",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "fk_formats_image_collections_image_collection_id",
                table: "formats",
                column: "image_collection_id",
                principalTable: "image_collections",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_formats_image_collections_image_collection_id",
                table: "formats");

            migrationBuilder.AlterColumn<int>(
                name: "manga_entry_id",
                table: "formats",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_formats_image_collections_image_collection_id",
                table: "formats",
                column: "image_collection_id",
                principalTable: "image_collections",
                principalColumn: "id");
        }
    }
}
