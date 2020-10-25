using Microsoft.EntityFrameworkCore.Migrations;

namespace RESTServer.Migrations
{
    public partial class UpdateUserAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasAdmin",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasAdmin",
                table: "Users");
        }
    }
}
