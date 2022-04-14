using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EnterpriseAssignment.Migrations
{
    public partial class CreateSessionQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "iduser",
                table: "session",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateTable(
                name: "sessionQuestion",
                columns: table => new
                {
                    Idsessionquestion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    iduser = table.Column<int>(type: "int", nullable: true),
                    idquestion = table.Column<int>(type: "int", nullable: true),
                    orderIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Idsessionquestion);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "IX_question_idcategory",
                table: "question",
                column: "idcategory");

            migrationBuilder.AddForeignKey(
                name: "FK_question_category_idcategory",
                table: "question",
                column: "idcategory",
                principalTable: "category",
                principalColumn: "idcategory",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_question_category_idcategory",
                table: "question");

            migrationBuilder.DropTable(
                name: "sessionQuestion");

            migrationBuilder.DropIndex(
                name: "IX_question_idcategory",
                table: "question");

            migrationBuilder.AlterColumn<int>(
                name: "iduser",
                table: "session",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);
        }
    }
}
