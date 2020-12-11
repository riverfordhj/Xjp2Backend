using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.Migrations
{
    public partial class fieldsAboutEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EditTime",
                table: "Persons",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Editor",
                table: "Persons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Persons",
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
        }
    }
}
