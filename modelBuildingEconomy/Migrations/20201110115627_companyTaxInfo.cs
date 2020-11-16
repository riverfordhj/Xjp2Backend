using Microsoft.EntityFrameworkCore.Migrations;

namespace ModelsBuildingEconomy.Migrations
{
    public partial class companyTaxInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_CompanyTaxInfo_CompanyTaxInfoId",
                table: "Company");

            migrationBuilder.DropIndex(
                name: "IX_Company_CompanyTaxInfoId",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "CompanyTaxInfoId",
                table: "Company");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "CompanyTaxInfo",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyTaxInfo_CompanyId",
                table: "CompanyTaxInfo",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyTaxInfo_Company_CompanyId",
                table: "CompanyTaxInfo",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyTaxInfo_Company_CompanyId",
                table: "CompanyTaxInfo");

            migrationBuilder.DropIndex(
                name: "IX_CompanyTaxInfo_CompanyId",
                table: "CompanyTaxInfo");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "CompanyTaxInfo");

            migrationBuilder.AddColumn<int>(
                name: "CompanyTaxInfoId",
                table: "Company",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Company_CompanyTaxInfoId",
                table: "Company",
                column: "CompanyTaxInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Company_CompanyTaxInfo_CompanyTaxInfoId",
                table: "Company",
                column: "CompanyTaxInfoId",
                principalTable: "CompanyTaxInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
