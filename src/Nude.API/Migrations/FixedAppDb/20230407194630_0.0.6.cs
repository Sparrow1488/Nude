using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nude.API.Migrations.FixedAppDb
{
    public partial class _006 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscribers_ReceiveRequests_ReceiveContentTicketId",
                table: "Subscribers");

            migrationBuilder.RenameColumn(
                name: "ReceiveContentTicketId",
                table: "Subscribers",
                newName: "ContentTicketId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscribers_ReceiveContentTicketId",
                table: "Subscribers",
                newName: "IX_Subscribers_ContentTicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribers_ReceiveRequests_ContentTicketId",
                table: "Subscribers",
                column: "ContentTicketId",
                principalTable: "ReceiveRequests",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscribers_ReceiveRequests_ContentTicketId",
                table: "Subscribers");

            migrationBuilder.RenameColumn(
                name: "ContentTicketId",
                table: "Subscribers",
                newName: "ReceiveContentTicketId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscribers_ContentTicketId",
                table: "Subscribers",
                newName: "IX_Subscribers_ReceiveContentTicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribers_ReceiveRequests_ReceiveContentTicketId",
                table: "Subscribers",
                column: "ReceiveContentTicketId",
                principalTable: "ReceiveRequests",
                principalColumn: "Id");
        }
    }
}
