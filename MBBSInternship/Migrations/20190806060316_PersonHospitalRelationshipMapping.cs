using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MBBSInternship.Migrations
{
    public partial class PersonHospitalRelationshipMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Persons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NIC",
                table: "Persons",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rank",
                table: "Persons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalSlots",
                table: "Hospitals",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PersonHospitalRelationships",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PreferenceNumber = table.Column<int>(nullable: false),
                    PersonId = table.Column<int>(nullable: false),
                    HospitalId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonHospitalRelationships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonHospitalRelationships_Hospitals_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospitals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonHospitalRelationships_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonHospitalRelationships_HospitalId",
                table: "PersonHospitalRelationships",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonHospitalRelationships_PersonId",
                table: "PersonHospitalRelationships",
                column: "PersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonHospitalRelationships");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "NIC",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "TotalSlots",
                table: "Hospitals");
        }
    }
}
