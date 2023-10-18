using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineAptitudeTest.Migrations
{
    public partial class V38 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Condidates",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Condidates");
        }
    }
}
