using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DjurparkGUI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Besökare",
                columns: table => new
                {
                    BesökareId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Namn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ålder = table.Column<int>(type: "int", nullable: false),
                    Epost = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefonnummer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Besökare", x => x.BesökareId);
                });

            migrationBuilder.CreateTable(
                name: "Habitats",
                columns: table => new
                {
                    HabitatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Namn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Växtlighet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Klimat = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Habitats", x => x.HabitatId);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    SettingsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntréPris = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxAntalBesökare = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.SettingsId);
                });

            migrationBuilder.CreateTable(
                name: "Besök",
                columns: table => new
                {
                    BesökId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BesökareId = table.Column<int>(type: "int", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Betald = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Besök", x => x.BesökId);
                    table.ForeignKey(
                        name: "FK_Besök_Besökare_BesökareId",
                        column: x => x.BesökareId,
                        principalTable: "Besökare",
                        principalColumn: "BesökareId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Djur",
                columns: table => new
                {
                    DjurId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Namn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Art = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Födelsedatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Kön = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Matkostnad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Omvårdnadskostnad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Popularitet = table.Column<int>(type: "int", nullable: false),
                    HabitatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Djur", x => x.DjurId);
                    table.ForeignKey(
                        name: "FK_Djur_Habitats_HabitatId",
                        column: x => x.HabitatId,
                        principalTable: "Habitats",
                        principalColumn: "HabitatId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Besök_BesökareId",
                table: "Besök",
                column: "BesökareId");

            migrationBuilder.CreateIndex(
                name: "IX_Djur_HabitatId",
                table: "Djur",
                column: "HabitatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Besök");

            migrationBuilder.DropTable(
                name: "Djur");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Besökare");

            migrationBuilder.DropTable(
                name: "Habitats");
        }
    }
}
