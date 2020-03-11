using Microsoft.EntityFrameworkCore.Migrations;

namespace EvaSystem.Data.Migrations
{
    public partial class Resettable5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_interectedUsers",
                table: "interectedUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "interectedUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "interectedUsers",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_interectedUsers",
                table: "interectedUsers",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_interectedUsers",
                table: "interectedUsers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "interectedUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "interectedUsers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_interectedUsers",
                table: "interectedUsers",
                column: "UserName");
        }
    }
}
