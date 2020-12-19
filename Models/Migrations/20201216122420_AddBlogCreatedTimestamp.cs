using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.Migrations
{
    public partial class AddBlogCreatedTimestamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersonHouseDatas",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    EthnicGroups = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    DomicileAddress = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    PoliticalState = table.Column<string>(nullable: true),
                    OrganizationalRelation = table.Column<string>(nullable: true),
                    IsOverseasChinese = table.Column<bool>(nullable: false),
                    MerriedStatus = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    IsHouseholder = table.Column<bool>(nullable: false),
                    RelationWithHouseholder = table.Column<string>(nullable: true),
                    IsOwner = table.Column<bool>(nullable: false),
                    IsLiveHere = table.Column<bool>(nullable: false),
                    PopulationCharacter = table.Column<string>(nullable: true),
                    LodgingReason = table.Column<string>(nullable: true),
                    RoomName = table.Column<string>(nullable: false),
                    Alias = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    RoomUse = table.Column<string>(nullable: true),
                    Area = table.Column<string>(nullable: true),
                    Longitude = table.Column<double>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Height = table.Column<double>(nullable: false),
                    Other = table.Column<string>(nullable: true),
                    RoomNote = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonHouseDatas", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonHouseDatas");
        }
    }
}
