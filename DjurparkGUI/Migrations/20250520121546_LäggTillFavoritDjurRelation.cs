using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DjurparkGUI.Migrations
{
    /// <inheritdoc />
    public partial class LäggTillFavoritDjurRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavoritDjur",
                columns: table => new
                {
                    BesökareId = table.Column<int>(type: "int", nullable: false),
                    DjurId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoritDjur", x => new { x.BesökareId, x.DjurId });
                    table.ForeignKey(
                        name: "FK_FavoritDjur_Besökare_BesökareId",
                        column: x => x.BesökareId,
                        principalTable: "Besökare",
                        principalColumn: "BesökareId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoritDjur_Djur_DjurId",
                        column: x => x.DjurId,
                        principalTable: "Djur",
                        principalColumn: "DjurId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoritDjur_DjurId",
                table: "FavoritDjur",
                column: "DjurId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoritDjur");
        }
    }
}
