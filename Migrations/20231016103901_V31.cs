using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineAptitudeTest.Migrations
{
    public partial class V31 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "catePartId",
                table: "resultHistories",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "QuestionHistoryId",
                table: "resultHistories",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "occupationId",
                table: "QuestionHistories",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_resultHistories_catePartId",
                table: "resultHistories",
                column: "catePartId");

            migrationBuilder.CreateIndex(
                name: "IX_resultHistories_QuestionHistoryId",
                table: "resultHistories",
                column: "QuestionHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionHistories_occupationId",
                table: "QuestionHistories",
                column: "occupationId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionHistories_Occupations_occupationId",
                table: "QuestionHistories",
                column: "occupationId",
                principalTable: "Occupations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_resultHistories_CateParts_catePartId",
                table: "resultHistories",
                column: "catePartId",
                principalTable: "CateParts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_resultHistories_QuestionHistories_QuestionHistoryId",
                table: "resultHistories",
                column: "QuestionHistoryId",
                principalTable: "QuestionHistories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionHistories_Occupations_occupationId",
                table: "QuestionHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_resultHistories_CateParts_catePartId",
                table: "resultHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_resultHistories_QuestionHistories_QuestionHistoryId",
                table: "resultHistories");

            migrationBuilder.DropIndex(
                name: "IX_resultHistories_catePartId",
                table: "resultHistories");

            migrationBuilder.DropIndex(
                name: "IX_resultHistories_QuestionHistoryId",
                table: "resultHistories");

            migrationBuilder.DropIndex(
                name: "IX_QuestionHistories_occupationId",
                table: "QuestionHistories");

            migrationBuilder.DropColumn(
                name: "QuestionHistoryId",
                table: "resultHistories");

            migrationBuilder.AlterColumn<string>(
                name: "catePartId",
                table: "resultHistories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "occupationId",
                table: "QuestionHistories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");
        }
    }
}
