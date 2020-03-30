using Microsoft.EntityFrameworkCore.Migrations;

namespace EvaSystem.Data.Migrations
{
    public partial class Updatescoretable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Scores");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Scores",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scores_UserId",
                table: "Scores",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_AspNetUsers_UserId",
                table: "Scores",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scores_AspNetUsers_UserId",
                table: "Scores");

            migrationBuilder.DropIndex(
                name: "IX_Scores_UserId",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Scores");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Scores",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
