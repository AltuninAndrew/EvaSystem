using Microsoft.EntityFrameworkCore.Migrations;

namespace EvaSystem.Data.Migrations
{
    public partial class Resettable6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_interectedUsers",
                table: "interectedUsers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "interectedUsers");

            migrationBuilder.AddColumn<int>(
                name: "EntryHash",
                table: "interectedUsers",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

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

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "interectedUsers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_interectedUsers",
                table: "interectedUsers",
                column: "Id");
        }
    }
}
