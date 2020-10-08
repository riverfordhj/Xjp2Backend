using Microsoft.EntityFrameworkCore.Migrations;

namespace ModelsBuildingEconomy.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company_OtherInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(nullable: true),
                    UnifiedSocialCreditCode = table.Column<string>(nullable: true),
                    Floor = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    Area = table.Column<string>(nullable: true),
                    SettlingTime = table.Column<string>(nullable: true),
                    MoveAwayTime = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company_OtherInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyBuilding",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuildingName = table.Column<string>(nullable: true),
                    StreetName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyBuilding", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyEconomy",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(nullable: true),
                    UnifiedSocialCreditCode = table.Column<string>(nullable: true),
                    CorporateTax = table.Column<string>(nullable: true),
                    duration = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyEconomy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(nullable: true),
                    UnifiedSocialCreditCode = table.Column<string>(nullable: true),
                    RegisteredAddress = table.Column<string>(nullable: true),
                    ActualOfficeAddress = table.Column<string>(nullable: true),
                    RegisteredCapital = table.Column<string>(nullable: true),
                    IsIndependentLegalEntity = table.Column<string>(nullable: true),
                    LegalRepresentative = table.Column<string>(nullable: true),
                    Contacts = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    EnterpriseType = table.Column<string>(nullable: true),
                    EnterpriseBackground = table.Column<string>(nullable: true),
                    BusinessDirection = table.Column<string>(nullable: true),
                    RegistrationPlace = table.Column<string>(nullable: true),
                    TaxStatisticsArea = table.Column<string>(nullable: true),
                    note = table.Column<string>(nullable: true),
                    CompanyBuildingId = table.Column<int>(nullable: true),
                    CompanyEconomyId = table.Column<int>(nullable: true),
                    Company_OtherInfoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Company_CompanyBuilding_CompanyBuildingId",
                        column: x => x.CompanyBuildingId,
                        principalTable: "CompanyBuilding",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Company_CompanyEconomy_CompanyEconomyId",
                        column: x => x.CompanyEconomyId,
                        principalTable: "CompanyEconomy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Company_Company_OtherInfo_Company_OtherInfoId",
                        column: x => x.Company_OtherInfoId,
                        principalTable: "Company_OtherInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Company_CompanyBuildingId",
                table: "Company",
                column: "CompanyBuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_CompanyEconomyId",
                table: "Company",
                column: "CompanyEconomyId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Company_OtherInfoId",
                table: "Company",
                column: "Company_OtherInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "CompanyBuilding");

            migrationBuilder.DropTable(
                name: "CompanyEconomy");

            migrationBuilder.DropTable(
                name: "Company_OtherInfo");
        }
    }
}
