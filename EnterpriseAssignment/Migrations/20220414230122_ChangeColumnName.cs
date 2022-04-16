using Microsoft.EntityFrameworkCore.Migrations;

namespace EnterpriseAssignment.Migrations
{
    public partial class ChangeColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Idsessionquestion",
                table: "sessionQuestion",
                newName: "idsessionquestion");

            migrationBuilder.RenameColumn(
                name: "iduser",
                table: "sessionQuestion",
                newName: "idsession");

            migrationBuilder.RenameColumn(
                name: "iduser",
                table: "session",
                newName: "idsession");

            migrationBuilder.RenameColumn(
                name: "iduser",
                table: "answer",
                newName: "idsession");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "idsessionquestion",
                table: "sessionQuestion",
                newName: "Idsessionquestion");

            migrationBuilder.RenameColumn(
                name: "idsession",
                table: "sessionQuestion",
                newName: "iduser");

            migrationBuilder.RenameColumn(
                name: "idsession",
                table: "session",
                newName: "iduser");

            migrationBuilder.RenameColumn(
                name: "idsession",
                table: "answer",
                newName: "iduser");
        }
    }
}
