using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineAptitudeTest.Migrations
{
    public partial class V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeOut",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "TimeType",
                table: "Questions");

            migrationBuilder.AddColumn<int>(
                name: "TimeOut",
                table: "CateParts",
                type: "int",
                maxLength: 11,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TimeType",
                table: "CateParts",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeOut",
                table: "CateParts");

            migrationBuilder.DropColumn(
                name: "TimeType",
                table: "CateParts");

            migrationBuilder.AddColumn<int>(
                name: "TimeOut",
                table: "Questions",
                type: "int",
                maxLength: 11,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TimeType",
                table: "Questions",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }
    }
}
