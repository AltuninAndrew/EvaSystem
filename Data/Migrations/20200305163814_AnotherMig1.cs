using Microsoft.EntityFrameworkCore.Migrations;

namespace EvaSystem.Data.Migrations
{
    public partial class AnotherMig1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "interectedUsers",
                columns: table => new
                {
                    UserName = table.Column<string>(nullable: false),
                    InterectedUserName = table.Column<string>(nullable: true),
                    UserModelId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_interectedUsers", x => x.UserName);
                    table.ForeignKey(
                        name: "FK_interectedUsers_AspNetUsers_UserModelId",
                        column: x => x.UserModelId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_interectedUsers_UserModelId",
                table: "interectedUsers",
                column: "UserModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "interectedUsers");
        }
    }
}
