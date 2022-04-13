using Microsoft.EntityFrameworkCore.Migrations;

namespace EnterpriseAssignment.Migrations
{
    public partial class InitialCreateTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "category",
                newName: "CategoryName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "category",
                newName: "name");
        }
    }
}
