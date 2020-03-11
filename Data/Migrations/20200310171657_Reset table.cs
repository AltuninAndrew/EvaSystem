using Microsoft.EntityFrameworkCore.Migrations;

namespace EvaSystem.Data.Migrations
{
    public partial class Resettable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogPostCounts",
                columns: table => new
                {
                    UserName = table.Column<string>(nullable: true),
                    InterectedUserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogPostCounts");
        }
    }
}
