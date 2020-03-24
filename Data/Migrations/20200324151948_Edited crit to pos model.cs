using Microsoft.EntityFrameworkCore.Migrations;

namespace EvaSystem.Data.Migrations
{
    public partial class Editedcrittoposmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CriterionsToPosition_Criterions_CriterionId",
                table: "CriterionsToPosition");

            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Criterions_CriterionId",
                table: "Scores");

            migrationBuilder.DropIndex(
                name: "IX_Scores_CriterionId",
                table: "Scores");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CriterionsToPosition_CriterionId_PositionId",
                table: "CriterionsToPosition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Criterions",
                table: "Criterions");

            migrationBuilder.DropColumn(
                name: "CriterionId",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "CriterionId",
                table: "CriterionsToPosition");

            migrationBuilder.DropColumn(
                name: "CriterionId",
                table: "Criterions");

            migrationBuilder.AddColumn<string>(
                name: "CriterionName",
                table: "Scores",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CriterionName",
                table: "CriterionsToPosition",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "CriterionName",
                table: "Criterions",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CriterionsToPosition_CriterionName_PositionId",
                table: "CriterionsToPosition",
                columns: new[] { "CriterionName", "PositionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Criterions",
                table: "Criterions",
                column: "CriterionName");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_CriterionName",
                table: "Scores",
                column: "CriterionName");

            migrationBuilder.AddForeignKey(
                name: "FK_CriterionsToPosition_Criterions_CriterionName",
                table: "CriterionsToPosition",
                column: "CriterionName",
                principalTable: "Criterions",
                principalColumn: "CriterionName",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Criterions_CriterionName",
                table: "Scores",
                column: "CriterionName",
                principalTable: "Criterions",
                principalColumn: "CriterionName",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CriterionsToPosition_Criterions_CriterionName",
                table: "CriterionsToPosition");

            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Criterions_CriterionName",
                table: "Scores");

            migrationBuilder.DropIndex(
                name: "IX_Scores_CriterionName",
                table: "Scores");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CriterionsToPosition_CriterionName_PositionId",
                table: "CriterionsToPosition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Criterions",
                table: "Criterions");

            migrationBuilder.DropColumn(
                name: "CriterionName",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "CriterionName",
                table: "CriterionsToPosition");

            migrationBuilder.AddColumn<int>(
                name: "CriterionId",
                table: "Scores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CriterionId",
                table: "CriterionsToPosition",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "CriterionName",
                table: "Criterions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "CriterionId",
                table: "Criterions",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CriterionsToPosition_CriterionId_PositionId",
                table: "CriterionsToPosition",
                columns: new[] { "CriterionId", "PositionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Criterions",
                table: "Criterions",
                column: "CriterionId");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_CriterionId",
                table: "Scores",
                column: "CriterionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CriterionsToPosition_Criterions_CriterionId",
                table: "CriterionsToPosition",
                column: "CriterionId",
                principalTable: "Criterions",
                principalColumn: "CriterionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Criterions_CriterionId",
                table: "Scores",
                column: "CriterionId",
                principalTable: "Criterions",
                principalColumn: "CriterionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
