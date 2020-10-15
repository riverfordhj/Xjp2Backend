using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.Migrations
{
    public partial class AddBlogCreatedTimestamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommunityId",
                table: "Subdivisions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subdivisions_CommunityId",
                table: "Subdivisions",
                column: "CommunityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subdivisions_Communitys_CommunityId",
                table: "Subdivisions",
                column: "CommunityId",
                principalTable: "Communitys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subdivisions_Communitys_CommunityId",
                table: "Subdivisions");

            migrationBuilder.DropIndex(
                name: "IX_Subdivisions_CommunityId",
                table: "Subdivisions");

            migrationBuilder.DropColumn(
                name: "CommunityId",
                table: "Subdivisions");
        }
    }
}
