using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nude.API.Migrations
{
    public partial class _0012 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_view_image_collections_image_collection_id",
                table: "view");

            migrationBuilder.DropForeignKey(
                name: "fk_view_images_image_id",
                table: "view");

            migrationBuilder.DropForeignKey(
                name: "fk_view_mangas_manga_id",
                table: "view");

            migrationBuilder.DropForeignKey(
                name: "fk_view_users_user_id",
                table: "view");

            migrationBuilder.DropPrimaryKey(
                name: "pk_view",
                table: "view");

            migrationBuilder.RenameTable(
                name: "view",
                newName: "views");

            migrationBuilder.RenameIndex(
                name: "ix_view_user_id",
                table: "views",
                newName: "ix_views_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_view_manga_id",
                table: "views",
                newName: "ix_views_manga_id");

            migrationBuilder.RenameIndex(
                name: "ix_view_image_id",
                table: "views",
                newName: "ix_views_image_id");

            migrationBuilder.RenameIndex(
                name: "ix_view_image_collection_id",
                table: "views",
                newName: "ix_views_image_collection_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_views",
                table: "views",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_views_image_collections_image_collection_id",
                table: "views",
                column: "image_collection_id",
                principalTable: "image_collections",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_views_images_image_id",
                table: "views",
                column: "image_id",
                principalTable: "images",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_views_mangas_manga_id",
                table: "views",
                column: "manga_id",
                principalTable: "mangas",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_views_users_user_id",
                table: "views",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_views_image_collections_image_collection_id",
                table: "views");

            migrationBuilder.DropForeignKey(
                name: "fk_views_images_image_id",
                table: "views");

            migrationBuilder.DropForeignKey(
                name: "fk_views_mangas_manga_id",
                table: "views");

            migrationBuilder.DropForeignKey(
                name: "fk_views_users_user_id",
                table: "views");

            migrationBuilder.DropPrimaryKey(
                name: "pk_views",
                table: "views");

            migrationBuilder.RenameTable(
                name: "views",
                newName: "view");

            migrationBuilder.RenameIndex(
                name: "ix_views_user_id",
                table: "view",
                newName: "ix_view_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_views_manga_id",
                table: "view",
                newName: "ix_view_manga_id");

            migrationBuilder.RenameIndex(
                name: "ix_views_image_id",
                table: "view",
                newName: "ix_view_image_id");

            migrationBuilder.RenameIndex(
                name: "ix_views_image_collection_id",
                table: "view",
                newName: "ix_view_image_collection_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_view",
                table: "view",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_view_image_collections_image_collection_id",
                table: "view",
                column: "image_collection_id",
                principalTable: "image_collections",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_view_images_image_id",
                table: "view",
                column: "image_id",
                principalTable: "images",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_view_mangas_manga_id",
                table: "view",
                column: "manga_id",
                principalTable: "mangas",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_view_users_user_id",
                table: "view",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
