using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.Migrations
{
    public partial class AddBlogCreatedTimestamp1215 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EditTime",
                table: "Persons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Editor",
                table: "Persons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Persons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "PersonRooms",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Status",
                table: "PersonRooms");
        }
    }
}
