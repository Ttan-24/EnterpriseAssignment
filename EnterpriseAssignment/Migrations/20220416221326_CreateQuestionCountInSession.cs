using Microsoft.EntityFrameworkCore.Migrations;

namespace EnterpriseAssignment.Migrations
{
    public partial class CreateQuestionCountInSession : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionCount",
                table: "session",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionCount",
                table: "session");
        }
    }
}
