using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.Migrations
{
    public partial class fixPersonHouseData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BuildingNam",
                table: "PersonHouseDatas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommunityName",
                table: "PersonHouseDatas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NetGrid",
                table: "PersonHouseDatas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuildingNam",
                table: "PersonHouseDatas");

            migrationBuilder.DropColumn(
                name: "CommunityName",
                table: "PersonHouseDatas");

            migrationBuilder.DropColumn(
                name: "NetGrid",
                table: "PersonHouseDatas");
        }
    }
}
