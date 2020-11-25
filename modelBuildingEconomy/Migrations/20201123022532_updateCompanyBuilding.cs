using Microsoft.EntityFrameworkCore.Migrations;

namespace ModelsBuildingEconomy.Migrations
{
    public partial class updateCompanyBuilding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompletionTime",
                table: "CompanyBuilding",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConstructionSite",
                table: "CompanyBuilding",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LegalEntity",
                table: "CompanyBuilding",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StartTime",
                table: "CompanyBuilding",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "CompanyBuilding",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletionTime",
                table: "CompanyBuilding");

            migrationBuilder.DropColumn(
                name: "ConstructionSite",
                table: "CompanyBuilding");

            migrationBuilder.DropColumn(
                name: "LegalEntity",
                table: "CompanyBuilding");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "CompanyBuilding");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "CompanyBuilding");
        }
    }
}
