using Microsoft.EntityFrameworkCore.Migrations;

namespace EvaSystem.Data.Migrations
{
    public partial class Addednewtables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Criterions",
                columns: table => new
                {
                    CriterionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CriterionName = table.Column<string>(nullable: true),
                    Weight = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Criterions", x => x.CriterionId);
                });

            migrationBuilder.CreateTable(
                name: "CriterionsToPosition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PositionId = table.Column<int>(nullable: false),
                    CriterionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CriterionsToPosition", x => x.Id);
                    table.UniqueConstraint("AK_CriterionsToPosition_CriterionId_PositionId", x => new { x.CriterionId, x.PositionId });
                    table.ForeignKey(
                        name: "FK_CriterionsToPosition_Criterions_CriterionId",
                        column: x => x.CriterionId,
                        principalTable: "Criterions",
                        principalColumn: "CriterionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CriterionsToPosition_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "PositionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Scores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(nullable: true),
                    CriterionId = table.Column<int>(nullable: false),
                    Score = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scores_Criterions_CriterionId",
                        column: x => x.CriterionId,
                        principalTable: "Criterions",
                        principalColumn: "CriterionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CriterionsToPosition_PositionId",
                table: "CriterionsToPosition",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_CriterionId",
                table: "Scores",
                column: "CriterionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CriterionsToPosition");

            migrationBuilder.DropTable(
                name: "Scores");

            migrationBuilder.DropTable(
                name: "Criterions");
        }
    }
}
