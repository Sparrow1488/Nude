using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nude.API.Migrations.FixedAppDb
{
    public partial class _005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscribers_ReceiveRequests_ReceiveContentRequestId",
                table: "Subscribers");

            migrationBuilder.RenameColumn(
                name: "ReceiveContentRequestId",
                table: "Subscribers",
                newName: "ReceiveContentTicketId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscribers_ReceiveContentRequestId",
                table: "Subscribers",
                newName: "IX_Subscribers_ReceiveContentTicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribers_ReceiveRequests_ReceiveContentTicketId",
                table: "Subscribers",
                column: "ReceiveContentTicketId",
                principalTable: "ReceiveRequests",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscribers_ReceiveRequests_ReceiveContentTicketId",
                table: "Subscribers");

            migrationBuilder.RenameColumn(
                name: "ReceiveContentTicketId",
                table: "Subscribers",
                newName: "ReceiveContentRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscribers_ReceiveContentTicketId",
                table: "Subscribers",
                newName: "IX_Subscribers_ReceiveContentRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribers_ReceiveRequests_ReceiveContentRequestId",
                table: "Subscribers",
                column: "ReceiveContentRequestId",
                principalTable: "ReceiveRequests",
                principalColumn: "Id");
        }
    }
}
