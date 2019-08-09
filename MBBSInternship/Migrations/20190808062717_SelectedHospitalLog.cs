using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MBBSInternship.Migrations
{
    public partial class SelectedHospitalLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SelectedHospitalLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LogNumber = table.Column<int>(nullable: false),
                    LogTime = table.Column<DateTime>(nullable: false),
                    CurrentTime = table.Column<DateTime>(nullable: false),
                    PersonId = table.Column<int>(nullable: false),
                    HospitalId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectedHospitalLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SelectedHospitalLogs_Hospitals_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospitals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SelectedHospitalLogs_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SelectedHospitalLogs_HospitalId",
                table: "SelectedHospitalLogs",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedHospitalLogs_PersonId",
                table: "SelectedHospitalLogs",
                column: "PersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SelectedHospitalLogs");
        }
    }
}
