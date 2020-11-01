using Microsoft.EntityFrameworkCore.Migrations;

namespace ModelsBuildingEconomy.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "CompanyOtherInfo",
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
                    table.PrimaryKey("PK_CompanyOtherInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyTaxInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnifiedSocialCreditCode = table.Column<string>(nullable: true),
                    TaxPayer = table.Column<string>(nullable: true),
                    TaxYear = table.Column<int>(nullable: false),
                    TotalTax = table.Column<double>(nullable: false),
                    BusinessTax = table.Column<double>(nullable: false),
                    ValueAddedTax = table.Column<double>(nullable: false),
                    CorporateIncomeTax = table.Column<double>(nullable: false),
                    IndividualIncomeTax = table.Column<double>(nullable: false),
                    UrbanConstructionTax = table.Column<double>(nullable: false),
                    RealEstateTax = table.Column<double>(nullable: false),
                    StampDuty = table.Column<double>(nullable: false),
                    LandUseTax = table.Column<double>(nullable: false),
                    LandValueIncrementTax = table.Column<double>(nullable: false),
                    VehicleAndVesselTax = table.Column<double>(nullable: false),
                    DeedTax = table.Column<double>(nullable: false),
                    AdditionalTaxOfEducation = table.Column<double>(nullable: false),
                    DelayedTaxPayment = table.Column<double>(nullable: false),
                    RegisteredAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyTaxInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BuildingFloor",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Community = table.Column<string>(nullable: true),
                    BuildingName = table.Column<string>(nullable: true),
                    FloorNum = table.Column<string>(nullable: true),
                    Long = table.Column<double>(nullable: false),
                    Lat = table.Column<double>(nullable: false),
                    Height = table.Column<double>(nullable: false),
                    CompanyBuildingId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingFloor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuildingFloor_CompanyBuilding_CompanyBuildingId",
                        column: x => x.CompanyBuildingId,
                        principalTable: "CompanyBuilding",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    CompanyOtherInfoId = table.Column<int>(nullable: true),
                    CompanyTaxInfoId = table.Column<int>(nullable: true)
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
                        name: "FK_Company_CompanyOtherInfo_CompanyOtherInfoId",
                        column: x => x.CompanyOtherInfoId,
                        principalTable: "CompanyOtherInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Company_CompanyTaxInfo_CompanyTaxInfoId",
                        column: x => x.CompanyTaxInfoId,
                        principalTable: "CompanyTaxInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BuildingFloor_CompanyBuildingId",
                table: "BuildingFloor",
                column: "CompanyBuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_CompanyBuildingId",
                table: "Company",
                column: "CompanyBuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_CompanyEconomyId",
                table: "Company",
                column: "CompanyEconomyId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_CompanyOtherInfoId",
                table: "Company",
                column: "CompanyOtherInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_CompanyTaxInfoId",
                table: "Company",
                column: "CompanyTaxInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuildingFloor");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "CompanyBuilding");

            migrationBuilder.DropTable(
                name: "CompanyEconomy");

            migrationBuilder.DropTable(
                name: "CompanyOtherInfo");

            migrationBuilder.DropTable(
                name: "CompanyTaxInfo");
        }
    }
}
