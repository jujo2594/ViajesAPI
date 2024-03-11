using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class migration9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Journeys_JourneyId",
                table: "Flights");

            migrationBuilder.DropTable(
                name: "JourneyFlights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_JourneyId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "JourneyId",
                table: "Flights");

            migrationBuilder.CreateTable(
                name: "FlightJourney",
                columns: table => new
                {
                    FlightsId = table.Column<int>(type: "int", nullable: false),
                    JourneysId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightJourney", x => new { x.FlightsId, x.JourneysId });
                    table.ForeignKey(
                        name: "FK_FlightJourney_Flights_FlightsId",
                        column: x => x.FlightsId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlightJourney_Journeys_JourneysId",
                        column: x => x.JourneysId,
                        principalTable: "Journeys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlightJourney_JourneysId",
                table: "FlightJourney",
                column: "JourneysId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlightJourney");

            migrationBuilder.AddColumn<int>(
                name: "JourneyId",
                table: "Flights",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JourneyFlights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightsId = table.Column<int>(type: "int", nullable: true),
                    JourneysId = table.Column<int>(type: "int", nullable: true),
                    IdFlightFk = table.Column<int>(type: "int", nullable: false),
                    IdJourneyFk = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JourneyFlights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JourneyFlights_Flights_FlightsId",
                        column: x => x.FlightsId,
                        principalTable: "Flights",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JourneyFlights_Journeys_JourneysId",
                        column: x => x.JourneysId,
                        principalTable: "Journeys",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flights_JourneyId",
                table: "Flights",
                column: "JourneyId");

            migrationBuilder.CreateIndex(
                name: "IX_JourneyFlights_FlightsId",
                table: "JourneyFlights",
                column: "FlightsId");

            migrationBuilder.CreateIndex(
                name: "IX_JourneyFlights_JourneysId",
                table: "JourneyFlights",
                column: "JourneysId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Journeys_JourneyId",
                table: "Flights",
                column: "JourneyId",
                principalTable: "Journeys",
                principalColumn: "Id");
        }
    }
}
