using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineAptitudeTest.Migrations
{
    public partial class V19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionHistories_Questions_QuestionId",
                table: "QuestionHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionHistories",
                table: "QuestionHistories");

            migrationBuilder.DropIndex(
                name: "IX_QuestionHistories_QuestionId",
                table: "QuestionHistories");

            migrationBuilder.DropColumn(
                name: "Answer",
                table: "QuestionHistories");

            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "QuestionHistories");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "QuestionHistories");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "QuestionHistories",
                newName: "userId");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "QuestionHistories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "QuestionHistories",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "occupationId",
                table: "QuestionHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionHistories",
                table: "QuestionHistories",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionHistories",
                table: "QuestionHistories");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "QuestionHistories");

            migrationBuilder.DropColumn(
                name: "occupationId",
                table: "QuestionHistories");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "QuestionHistories",
                newName: "UserId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "QuestionHistories",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Answer",
                table: "QuestionHistories",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "QuestionHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "QuestionId",
                table: "QuestionHistories",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionHistories",
                table: "QuestionHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionHistories_QuestionId",
                table: "QuestionHistories",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionHistories_Questions_QuestionId",
                table: "QuestionHistories",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
