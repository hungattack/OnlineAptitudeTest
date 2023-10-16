using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineAptitudeTest.Migrations
{
    public partial class V33 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CateParts_QuestionHistories_QuestionHistoryId",
                table: "CateParts");

            migrationBuilder.DropIndex(
                name: "IX_CateParts_QuestionHistoryId",
                table: "CateParts");

            migrationBuilder.DropColumn(
                name: "QuestionHistoryId",
                table: "CateParts");

            migrationBuilder.AddColumn<int>(
                name: "pointAll",
                table: "QuestionHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pointAll",
                table: "QuestionHistories");

            migrationBuilder.AddColumn<string>(
                name: "QuestionHistoryId",
                table: "CateParts",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CateParts_QuestionHistoryId",
                table: "CateParts",
                column: "QuestionHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CateParts_QuestionHistories_QuestionHistoryId",
                table: "CateParts",
                column: "QuestionHistoryId",
                principalTable: "QuestionHistories",
                principalColumn: "Id");
        }
    }
}
