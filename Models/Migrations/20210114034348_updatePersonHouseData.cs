using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.Migrations
{
    public partial class updatePersonHouseData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "PersonHouseDatas",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PersonHouseDatas",
                newName: "ID");
        }
    }
}
