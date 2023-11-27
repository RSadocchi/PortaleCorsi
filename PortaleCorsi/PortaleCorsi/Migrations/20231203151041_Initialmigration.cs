using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortaleCorsi.Migrations
{
    /// <inheritdoc />
    public partial class Initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnagraficaMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodiceFiscale = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Cognome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnagraficaMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CorsoMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataCreazione = table.Column<DateTime>(type: "date", nullable: false),
                    Codice = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descrizione = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataInizio = table.Column<DateTime>(type: "date", nullable: false),
                    OnLine = table.Column<bool>(type: "bit", nullable: false),
                    LuogoLezioni = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxPartecipanti = table.Column<int>(type: "int", nullable: false),
                    DataFineIscrizioni = table.Column<DateTime>(type: "datetime", nullable: false),
                    IscrizioniChiuse = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorsoMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnagraficaIndirizzo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnagraficaId = table.Column<int>(type: "int", nullable: false),
                    Indirizzo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Civico = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Citta = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Cap = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Prov = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnagraficaIndirizzo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnagraficaIndirizzo_AnagraficaMaster_AnagraficaId",
                        column: x => x.AnagraficaId,
                        principalTable: "AnagraficaMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CorsoIscrizione",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CorsoId = table.Column<int>(type: "int", nullable: false),
                    AnagraficaId = table.Column<int>(type: "int", nullable: false),
                    DataIscrizione = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorsoIscrizione", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CorsoIscrizione_AnagraficaMaster_AnagraficaId",
                        column: x => x.AnagraficaId,
                        principalTable: "AnagraficaMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CorsoIscrizione_CorsoMaster_CorsoId",
                        column: x => x.CorsoId,
                        principalTable: "CorsoMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CorsoLezione",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CorsoId = table.Column<int>(type: "int", nullable: false),
                    DataOraInizio = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataOraFine = table.Column<DateTime>(type: "datetime", nullable: false),
                    Descrizione = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorsoLezione", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CorsoLezione_CorsoMaster_CorsoId",
                        column: x => x.CorsoId,
                        principalTable: "CorsoMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnagraficaIndirizzo_AnagraficaId",
                table: "AnagraficaIndirizzo",
                column: "AnagraficaId");

            migrationBuilder.CreateIndex(
                name: "IX_AnagraficaMaster_CodiceFiscale",
                table: "AnagraficaMaster",
                column: "CodiceFiscale",
                unique: true)
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_CorsoIscrizione_AnagraficaId",
                table: "CorsoIscrizione",
                column: "AnagraficaId");

            migrationBuilder.CreateIndex(
                name: "IX_CorsoIscrizione_CorsoId",
                table: "CorsoIscrizione",
                column: "CorsoId");

            migrationBuilder.CreateIndex(
                name: "IX_CorsoLezione_CorsoId",
                table: "CorsoLezione",
                column: "CorsoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnagraficaIndirizzo");

            migrationBuilder.DropTable(
                name: "CorsoIscrizione");

            migrationBuilder.DropTable(
                name: "CorsoLezione");

            migrationBuilder.DropTable(
                name: "AnagraficaMaster");

            migrationBuilder.DropTable(
                name: "CorsoMaster");
        }
    }
}
