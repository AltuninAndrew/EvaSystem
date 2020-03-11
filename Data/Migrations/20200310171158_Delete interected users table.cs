using Microsoft.EntityFrameworkCore.Migrations;

namespace EvaSystem.Data.Migrations
{
    public partial class Deleteinterecteduserstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "interectedUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "interectedUsers",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InterectedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_interectedUsers", x => x.UserName);
                });
        }
    }
}
