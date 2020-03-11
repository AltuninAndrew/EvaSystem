using Microsoft.EntityFrameworkCore.Migrations;

namespace EvaSystem.Data.Migrations
{
    public partial class Resettable7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_interectedUsers",
                table: "interectedUsers",
                column: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_interectedUsers",
                table: "interectedUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "interectedUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "EntryHash",
                table: "interectedUsers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_interectedUsers",
                table: "interectedUsers",
                column: "EntryHash");
        }
    }
}
