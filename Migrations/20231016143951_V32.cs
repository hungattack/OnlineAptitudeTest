using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineAptitudeTest.Migrations
{
    public partial class V32 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
