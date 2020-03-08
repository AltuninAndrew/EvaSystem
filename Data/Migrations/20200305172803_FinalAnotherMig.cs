using Microsoft.EntityFrameworkCore.Migrations;

namespace EvaSystem.Data.Migrations
{
    public partial class FinalAnotherMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_interectedUsers_AspNetUsers_UserModelId",
                table: "interectedUsers");

            migrationBuilder.DropIndex(
                name: "IX_interectedUsers_UserModelId",
                table: "interectedUsers");

            migrationBuilder.DropColumn(
                name: "UserModelId",
                table: "interectedUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserModelId",
                table: "interectedUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_interectedUsers_UserModelId",
                table: "interectedUsers",
                column: "UserModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_interectedUsers_AspNetUsers_UserModelId",
                table: "interectedUsers",
                column: "UserModelId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
