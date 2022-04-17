using Microsoft.EntityFrameworkCore.Migrations;

namespace EnterpriseAssignment.Migrations
{
    public partial class CreateTextLocationHintUsed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HintsUsed",
                table: "sessionquestion",
                newName: "TextHintUsed");

            migrationBuilder.AddColumn<bool>(
                name: "LocationHintUsed",
                table: "sessionquestion",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationHintUsed",
                table: "sessionquestion");

            migrationBuilder.RenameColumn(
                name: "TextHintUsed",
                table: "sessionquestion",
                newName: "HintsUsed");
        }
    }
}
