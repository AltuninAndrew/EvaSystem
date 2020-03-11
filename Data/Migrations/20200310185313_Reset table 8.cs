using Microsoft.EntityFrameworkCore.Migrations;

namespace EvaSystem.Data.Migrations
{
    public partial class Resettable8 : Migration
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

            migrationBuilder.AddColumn<string>(
                name: "EntryHash",
                table: "interectedUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_interectedUsers",
                table: "interectedUsers",
                column: "EntryHash");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_interectedUsers",
                table: "interectedUsers");

            migrationBuilder.DropColumn(
                name: "EntryHash",
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
