using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.Migrations
{
    public partial class AddBlogCreatedTimestamp111 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EditTime",
                table: "PersonHouseDatas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Editor",
                table: "PersonHouseDatas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Operation",
                table: "PersonHouseDatas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "PersonHouseDatas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditTime",
                table: "PersonHouseDatas");

            migrationBuilder.DropColumn(
                name: "Editor",
                table: "PersonHouseDatas");

            migrationBuilder.DropColumn(
                name: "Operation",
                table: "PersonHouseDatas");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "PersonHouseDatas");
        }
    }
}
