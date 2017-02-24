using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DZ.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Passagier",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EMail = table.Column<string>(nullable: true),
                    Foto = table.Column<byte[]>(nullable: true),
                    Geburtsdatum = table.Column<DateTime>(nullable: true),
                    KundeSeit = table.Column<DateTime>(nullable: true),
                    Land = table.Column<string>(nullable: true),
                    Memo = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    PassagierStatus = table.Column<string>(nullable: true),
                    Stadt = table.Column<string>(nullable: true),
                    Strasse = table.Column<string>(nullable: true),
                    Vorname = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passagier", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Pilot",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EMail = table.Column<string>(nullable: true),
                    Fluglizenztyp = table.Column<int>(nullable: false),
                    FlugscheinSeit = table.Column<DateTime>(nullable: false),
                    Flugschule = table.Column<string>(nullable: true),
                    Flugstunden = table.Column<int>(nullable: true),
                    Foto = table.Column<byte[]>(nullable: true),
                    Geburtsdatum = table.Column<DateTime>(nullable: true),
                    Land = table.Column<string>(nullable: true),
                    Memo = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Stadt = table.Column<string>(nullable: true),
                    Strasse = table.Column<string>(nullable: true),
                    Vorname = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pilot", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Flug",
                columns: table => new
                {
                    FlugNr = table.Column<int>(nullable: false),
                    Abflugort = table.Column<string>(maxLength: 50, nullable: true),
                    CopilotId = table.Column<int>(nullable: false),
                    Datum = table.Column<DateTime>(nullable: false),
                    FreiePlaetze = table.Column<short>(nullable: true),
                    Memo = table.Column<string>(nullable: true),
                    NichtRaucherFlug = table.Column<bool>(nullable: false),
                    PilotId = table.Column<int>(nullable: false),
                    Plaetze = table.Column<short>(nullable: true),
                    Preis = table.Column<decimal>(nullable: false),
                    Zielort = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flug", x => x.FlugNr);
                    table.ForeignKey(
                        name: "FK_Flug_Pilot_CopilotId",
                        column: x => x.CopilotId,
                        principalTable: "Pilot",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flug_Pilot_PilotId",
                        column: x => x.PilotId,
                        principalTable: "Pilot",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Buchung",
                columns: table => new
                {
                    FlugNr = table.Column<int>(nullable: false),
                    PassagierID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buchung", x => new { x.FlugNr, x.PassagierID });
                    table.ForeignKey(
                        name: "FK_Buchung_Flug_FlugNr",
                        column: x => x.FlugNr,
                        principalTable: "Flug",
                        principalColumn: "FlugNr",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Buchung_Passagier_PassagierID",
                        column: x => x.PassagierID,
                        principalTable: "Passagier",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Buchung_PassagierID",
                table: "Buchung",
                column: "PassagierID");

            migrationBuilder.CreateIndex(
                name: "IX_Flug_Abflugort",
                table: "Flug",
                column: "Abflugort");

            migrationBuilder.CreateIndex(
                name: "IX_Flug_CopilotId",
                table: "Flug",
                column: "CopilotId");

            migrationBuilder.CreateIndex(
                name: "IX_Flug_PilotId",
                table: "Flug",
                column: "PilotId");

            migrationBuilder.CreateIndex(
                name: "IX_Flug_Zielort",
                table: "Flug",
                column: "Zielort");

            migrationBuilder.CreateIndex(
                name: "IX_Passagier_Name",
                table: "Passagier",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Passagier_Vorname",
                table: "Passagier",
                column: "Vorname");

            migrationBuilder.CreateIndex(
                name: "IX_Pilot_Name",
                table: "Pilot",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Pilot_Vorname",
                table: "Pilot",
                column: "Vorname");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Buchung");

            migrationBuilder.DropTable(
                name: "Flug");

            migrationBuilder.DropTable(
                name: "Passagier");

            migrationBuilder.DropTable(
                name: "Pilot");
        }
    }
}
