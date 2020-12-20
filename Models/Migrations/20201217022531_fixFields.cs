using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.Migrations
{
    public partial class fixFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuildingNam",
                table: "PersonHouseDatas");

            migrationBuilder.AddColumn<string>(
                name: "BuildingName",
                table: "PersonHouseDatas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuildingName",
                table: "PersonHouseDatas");

            migrationBuilder.AddColumn<string>(
                name: "BuildingNam",
                table: "PersonHouseDatas",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
