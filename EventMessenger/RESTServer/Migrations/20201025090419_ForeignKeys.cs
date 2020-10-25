using Microsoft.EntityFrameworkCore.Migrations;

namespace RESTServer.Migrations
{
    public partial class ForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Reminders",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_EventId",
                table: "Reminders",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_SeverityId",
                table: "Events",
                column: "SeverityId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_StatusId",
                table: "Events",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Severity_SeverityId",
                table: "Events",
                column: "SeverityId",
                principalTable: "Severity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Status_StatusId",
                table: "Events",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reminders_Events_EventId",
                table: "Reminders",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Severity_SeverityId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Status_StatusId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Reminders_Events_EventId",
                table: "Reminders");

            migrationBuilder.DropIndex(
                name: "IX_Reminders_EventId",
                table: "Reminders");

            migrationBuilder.DropIndex(
                name: "IX_Events_SeverityId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_StatusId",
                table: "Events");

            migrationBuilder.AlterColumn<string>(
                name: "EventId",
                table: "Reminders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
