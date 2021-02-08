using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.Migrations
{
    public partial class AddBlogCreatedTimestamp1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "PersonHouseDatas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StreetName",
                table: "PersonHouseDatas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubdivisionName",
                table: "PersonHouseDatas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "PersonHouseDatas");

            migrationBuilder.DropColumn(
                name: "StreetName",
                table: "PersonHouseDatas");

            migrationBuilder.DropColumn(
                name: "SubdivisionName",
                table: "PersonHouseDatas");
        }
    }
}
