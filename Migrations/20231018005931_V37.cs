using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineAptitudeTest.Migrations
{
    public partial class V37 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "managerId",
                table: "infos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "managerId",
                table: "infos");
        }
    }
}
