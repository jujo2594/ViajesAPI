using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class migration8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JourneyFlight_Flights_FlightsId",
                table: "JourneyFlight");

            migrationBuilder.DropForeignKey(
                name: "FK_JourneyFlight_Journeys_JourneysId",
                table: "JourneyFlight");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JourneyFlight",
                table: "JourneyFlight");

            migrationBuilder.RenameTable(
                name: "JourneyFlight",
                newName: "JourneyFlights");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "Journeys",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "OriginCustomer",
                table: "Journeys",
                newName: "Origin");

            migrationBuilder.RenameColumn(
                name: "DestinationCustomer",
                table: "Journeys",
                newName: "Destination");

            migrationBuilder.RenameColumn(
                name: "Origin",
                table: "Flights",
                newName: "DepartureStation");

            migrationBuilder.RenameColumn(
                name: "Destination",
                table: "Flights",
                newName: "ArrivalStation");

            migrationBuilder.RenameIndex(
                name: "IX_JourneyFlight_JourneysId",
                table: "JourneyFlights",
                newName: "IX_JourneyFlights_JourneysId");

            migrationBuilder.RenameIndex(
                name: "IX_JourneyFlight_FlightsId",
                table: "JourneyFlights",
                newName: "IX_JourneyFlights_FlightsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JourneyFlights",
                table: "JourneyFlights",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JourneyFlights_Flights_FlightsId",
                table: "JourneyFlights",
                column: "FlightsId",
                principalTable: "Flights",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JourneyFlights_Journeys_JourneysId",
                table: "JourneyFlights",
                column: "JourneysId",
                principalTable: "Journeys",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JourneyFlights_Flights_FlightsId",
                table: "JourneyFlights");

            migrationBuilder.DropForeignKey(
                name: "FK_JourneyFlights_Journeys_JourneysId",
                table: "JourneyFlights");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JourneyFlights",
                table: "JourneyFlights");

            migrationBuilder.RenameTable(
                name: "JourneyFlights",
                newName: "JourneyFlight");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Journeys",
                newName: "Total");

            migrationBuilder.RenameColumn(
                name: "Origin",
                table: "Journeys",
                newName: "OriginCustomer");

            migrationBuilder.RenameColumn(
                name: "Destination",
                table: "Journeys",
                newName: "DestinationCustomer");

            migrationBuilder.RenameColumn(
                name: "DepartureStation",
                table: "Flights",
                newName: "Origin");

            migrationBuilder.RenameColumn(
                name: "ArrivalStation",
                table: "Flights",
                newName: "Destination");

            migrationBuilder.RenameIndex(
                name: "IX_JourneyFlights_JourneysId",
                table: "JourneyFlight",
                newName: "IX_JourneyFlight_JourneysId");

            migrationBuilder.RenameIndex(
                name: "IX_JourneyFlights_FlightsId",
                table: "JourneyFlight",
                newName: "IX_JourneyFlight_FlightsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JourneyFlight",
                table: "JourneyFlight",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JourneyFlight_Flights_FlightsId",
                table: "JourneyFlight",
                column: "FlightsId",
                principalTable: "Flights",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JourneyFlight_Journeys_JourneysId",
                table: "JourneyFlight",
                column: "JourneysId",
                principalTable: "Journeys",
                principalColumn: "Id");
        }
    }
}
