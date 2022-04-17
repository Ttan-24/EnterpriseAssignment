using Microsoft.EntityFrameworkCore.Migrations;

namespace EnterpriseAssignment.Migrations
{
    public partial class CreateEndSessionHintsUsedColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HintsUsed",
                table: "sessionquestion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "EndSession",
                table: "session",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HintsUsed",
                table: "sessionquestion");

            migrationBuilder.DropColumn(
                name: "EndSession",
                table: "session");
        }
    }
}
