using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.Migrations
{
    public partial class addEditingPhone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EditingPhone",
                table: "Persons",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditingPhone",
                table: "Persons");
        }
    }
}
