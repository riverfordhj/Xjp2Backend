using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        //    migrationBuilder.CreateTable(
        //        name: "Disability",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            PersonId = table.Column<string>(nullable: true),
        //            Type = table.Column<string>(nullable: true),
        //            Class = table.Column<string>(nullable: true),
        //            Note = table.Column<string>(nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Disability", x => x.Id);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "MilitaryService",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            PersonId = table.Column<string>(nullable: true),
        //            Type = table.Column<string>(nullable: true),
        //            Note = table.Column<string>(nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_MilitaryService", x => x.Id);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "OtherInfos",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            PersonId = table.Column<string>(nullable: true),
        //            Key = table.Column<string>(nullable: true),
        //            Value = table.Column<string>(nullable: true),
        //            Note = table.Column<string>(nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_OtherInfos", x => x.Id);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "PersonHouseDatas",
        //        columns: table => new
        //        {
        //            ID = table.Column<int>(nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            PersonId = table.Column<string>(nullable: false),
        //            Name = table.Column<string>(nullable: false),
        //            EthnicGroups = table.Column<string>(nullable: true),
        //            Phone = table.Column<string>(nullable: true),
        //            DomicileAddress = table.Column<string>(nullable: true),
        //            Company = table.Column<string>(nullable: true),
        //            PoliticalState = table.Column<string>(nullable: true),
        //            OrganizationalRelation = table.Column<string>(nullable: true),
        //            IsOverseasChinese = table.Column<bool>(nullable: false),
        //            MerriedStatus = table.Column<string>(nullable: true),
        //            Note = table.Column<string>(nullable: true),
        //            IsHouseholder = table.Column<bool>(nullable: false),
        //            RelationWithHouseholder = table.Column<string>(nullable: true),
        //            IsOwner = table.Column<bool>(nullable: false),
        //            IsLiveHere = table.Column<bool>(nullable: false),
        //            PopulationCharacter = table.Column<string>(nullable: true),
        //            LodgingReason = table.Column<string>(nullable: true),
        //            RoomName = table.Column<string>(nullable: false),
        //            Alias = table.Column<string>(nullable: true),
        //            Category = table.Column<string>(nullable: true),
        //            RoomUse = table.Column<string>(nullable: true),
        //            Area = table.Column<string>(nullable: true),
        //            Longitude = table.Column<double>(nullable: false),
        //            Latitude = table.Column<double>(nullable: false),
        //            Height = table.Column<double>(nullable: false),
        //            Other = table.Column<string>(nullable: true),
        //            RoomNote = table.Column<string>(nullable: true),
        //            EditTime = table.Column<string>(nullable: true),
        //            Editor = table.Column<string>(nullable: true),
        //            Operation = table.Column<string>(nullable: true),
        //            Status = table.Column<string>(nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_PersonHouseDatas", x => x.ID);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Persons",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            PersonId = table.Column<string>(nullable: false),
        //            Name = table.Column<string>(nullable: false),
        //            EthnicGroups = table.Column<string>(nullable: true),
        //            Phone = table.Column<string>(nullable: true),
        //            DomicileAddress = table.Column<string>(nullable: true),
        //            Company = table.Column<string>(nullable: true),
        //            PoliticalState = table.Column<string>(nullable: true),
        //            OrganizationalRelation = table.Column<string>(nullable: true),
        //            IsOverseasChinese = table.Column<bool>(nullable: false),
        //            MerriedStatus = table.Column<string>(nullable: true),
        //            Note = table.Column<string>(nullable: true),
        //            EditTime = table.Column<string>(nullable: true),
        //            Editor = table.Column<string>(nullable: true),
        //            Status = table.Column<string>(nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Persons", x => x.Id);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "PoorPeoples",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            PersonId = table.Column<string>(nullable: true),
        //            Type = table.Column<string>(nullable: true),
        //            Child = table.Column<string>(nullable: true),
        //            Youngsters = table.Column<string>(nullable: true),
        //            SpecialHelp = table.Column<string>(nullable: true),
        //            Note = table.Column<string>(nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_PoorPeoples", x => x.Id);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Roles",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            Name = table.Column<string>(nullable: true),
        //            Note = table.Column<string>(nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Roles", x => x.Id);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Streets",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            Name = table.Column<string>(nullable: false),
        //            Alias = table.Column<string>(nullable: true),
        //            Note = table.Column<string>(nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Streets", x => x.Id);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Users",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            UserName = table.Column<string>(nullable: true),
        //            Password = table.Column<string>(nullable: true),
        //            phone = table.Column<string>(nullable: true),
        //            PicPath = table.Column<string>(nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Users", x => x.Id);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "SpecialGroups",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            PersonId = table.Column<string>(nullable: true),
        //            Type = table.Column<string>(nullable: true),
        //            Note = table.Column<string>(nullable: true),
        //            PersonId1 = table.Column<int>(nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_SpecialGroups", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_SpecialGroups_Persons_PersonId1",
        //                column: x => x.PersonId1,
        //                principalTable: "Persons",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Communitys",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            Name = table.Column<string>(nullable: false),
        //            Alias = table.Column<string>(nullable: true),
        //            Note = table.Column<string>(nullable: true),
        //            StreetId = table.Column<int>(nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Communitys", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_Communitys_Streets_StreetId",
        //                column: x => x.StreetId,
        //                principalTable: "Streets",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "RoleUsers",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            RoleId = table.Column<int>(nullable: false),
        //            UserId = table.Column<int>(nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_RoleUsers", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_RoleUsers_Roles_RoleId",
        //                column: x => x.RoleId,
        //                principalTable: "Roles",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //            table.ForeignKey(
        //                name: "FK_RoleUsers_Users_UserId",
        //                column: x => x.UserId,
        //                principalTable: "Users",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "NetGrids",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            Name = table.Column<string>(nullable: false),
        //            Alias = table.Column<string>(nullable: true),
        //            Note = table.Column<string>(nullable: true),
        //            CommunityId = table.Column<int>(nullable: true),
        //            UserId = table.Column<int>(nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_NetGrids", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_NetGrids_Communitys_CommunityId",
        //                column: x => x.CommunityId,
        //                principalTable: "Communitys",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_NetGrids_Users_UserId",
        //                column: x => x.UserId,
        //                principalTable: "Users",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Subdivisions",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            Name = table.Column<string>(nullable: false),
        //            Alias = table.Column<string>(nullable: true),
        //            Note = table.Column<string>(nullable: true),
        //            CommunityId = table.Column<int>(nullable: true),
        //            StreetId = table.Column<int>(nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Subdivisions", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_Subdivisions_Communitys_CommunityId",
        //                column: x => x.CommunityId,
        //                principalTable: "Communitys",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_Subdivisions_Streets_StreetId",
        //                column: x => x.StreetId,
        //                principalTable: "Streets",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Buildings",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            Name = table.Column<string>(nullable: false),
        //            Alias = table.Column<string>(nullable: true),
        //            Address = table.Column<string>(nullable: true),
        //            Note = table.Column<string>(nullable: true),
        //            NetGridId = table.Column<int>(nullable: true),
        //            SubdivisionId = table.Column<int>(nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Buildings", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_Buildings_NetGrids_NetGridId",
        //                column: x => x.NetGridId,
        //                principalTable: "NetGrids",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_Buildings_Subdivisions_SubdivisionId",
        //                column: x => x.SubdivisionId,
        //                principalTable: "Subdivisions",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Rooms",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            Name = table.Column<string>(nullable: false),
        //            Alias = table.Column<string>(nullable: true),
        //            Category = table.Column<string>(nullable: true),
        //            Use = table.Column<string>(nullable: true),
        //            Area = table.Column<string>(nullable: true),
        //            Longitude = table.Column<double>(nullable: false),
        //            Latitude = table.Column<double>(nullable: false),
        //            Height = table.Column<double>(nullable: false),
        //            Other = table.Column<string>(nullable: true),
        //            Note = table.Column<string>(nullable: true),
        //            BuildingId = table.Column<int>(nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Rooms", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_Rooms_Buildings_BuildingId",
        //                column: x => x.BuildingId,
        //                principalTable: "Buildings",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "CompanyInfos",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            Name = table.Column<string>(nullable: true),
        //            Character = table.Column<string>(nullable: true),
        //            SocialId = table.Column<string>(nullable: true),
        //            ContactPerson = table.Column<string>(nullable: true),
        //            PersonId = table.Column<string>(nullable: true),
        //            Phone = table.Column<string>(nullable: true),
        //            Area = table.Column<string>(nullable: true),
        //            Note = table.Column<string>(nullable: true),
        //            RoomId = table.Column<int>(nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_CompanyInfos", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_CompanyInfos_Rooms_RoomId",
        //                column: x => x.RoomId,
        //                principalTable: "Rooms",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "PersonRooms",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            PersonId = table.Column<string>(nullable: true),
        //            IsHouseholder = table.Column<bool>(nullable: false),
        //            RelationWithHouseholder = table.Column<string>(nullable: true),
        //            IsOwner = table.Column<bool>(nullable: false),
        //            IsLiveHere = table.Column<bool>(nullable: false),
        //            PopulationCharacter = table.Column<string>(nullable: true),
        //            LodgingReason = table.Column<string>(nullable: true),
        //            Status = table.Column<string>(nullable: true),
        //            PersonId1 = table.Column<int>(nullable: true),
        //            RoomId = table.Column<int>(nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_PersonRooms", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_PersonRooms_Persons_PersonId1",
        //                column: x => x.PersonId1,
        //                principalTable: "Persons",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_PersonRooms_Rooms_RoomId",
        //                column: x => x.RoomId,
        //                principalTable: "Rooms",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Buildings_NetGridId",
        //        table: "Buildings",
        //        column: "NetGridId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Buildings_SubdivisionId",
        //        table: "Buildings",
        //        column: "SubdivisionId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Communitys_StreetId",
        //        table: "Communitys",
        //        column: "StreetId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_CompanyInfos_RoomId",
        //        table: "CompanyInfos",
        //        column: "RoomId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_NetGrids_CommunityId",
        //        table: "NetGrids",
        //        column: "CommunityId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_NetGrids_UserId",
        //        table: "NetGrids",
        //        column: "UserId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_PersonRooms_PersonId1",
        //        table: "PersonRooms",
        //        column: "PersonId1");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_PersonRooms_RoomId",
        //        table: "PersonRooms",
        //        column: "RoomId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Persons_PersonId",
        //        table: "Persons",
        //        column: "PersonId",
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_RoleUsers_RoleId",
        //        table: "RoleUsers",
        //        column: "RoleId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_RoleUsers_UserId",
        //        table: "RoleUsers",
        //        column: "UserId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Rooms_BuildingId",
        //        table: "Rooms",
        //        column: "BuildingId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SpecialGroups_PersonId1",
        //        table: "SpecialGroups",
        //        column: "PersonId1");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Subdivisions_CommunityId",
        //        table: "Subdivisions",
        //        column: "CommunityId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Subdivisions_StreetId",
        //        table: "Subdivisions",
        //        column: "StreetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyInfos");

            migrationBuilder.DropTable(
                name: "Disability");

            migrationBuilder.DropTable(
                name: "MilitaryService");

            migrationBuilder.DropTable(
                name: "OtherInfos");

            migrationBuilder.DropTable(
                name: "PersonHouseDatas");

            migrationBuilder.DropTable(
                name: "PersonRooms");

            migrationBuilder.DropTable(
                name: "PoorPeoples");

            migrationBuilder.DropTable(
                name: "RoleUsers");

            migrationBuilder.DropTable(
                name: "SpecialGroups");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropTable(
                name: "NetGrids");

            migrationBuilder.DropTable(
                name: "Subdivisions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Communitys");

            migrationBuilder.DropTable(
                name: "Streets");
        }
    }
}
