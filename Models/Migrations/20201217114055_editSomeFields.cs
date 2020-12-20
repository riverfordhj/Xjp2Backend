using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.Migrations
{
    public partial class editSomeFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditTime",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "Editor",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Persons");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EditTime",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Editor",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
