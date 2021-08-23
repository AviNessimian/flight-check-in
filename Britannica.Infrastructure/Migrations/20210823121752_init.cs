using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Britannica.Infrastructure.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Origin = table.Column<string>(maxLength: 255, nullable: false),
                    Destination = table.Column<string>(maxLength: 255, nullable: false),
                    Depart = table.Column<DateTime>(nullable: false),
                    Return = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Passengers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    FirtName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passengers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Aircrafts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    WeightLimit = table.Column<decimal>(nullable: false),
                    BaggagesLimit = table.Column<ushort>(nullable: false),
                    FlightRef = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aircrafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aircrafts_Flights_FlightRef",
                        column: x => x.FlightRef,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PassengerFlights",
                columns: table => new
                {
                    FlightId = table.Column<int>(nullable: false),
                    PassengerId = table.Column<int>(nullable: false),
                    SeatId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassengerFlights", x => new { x.FlightId, x.PassengerId });
                    table.ForeignKey(
                        name: "FK_PassengerFlights_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PassengerFlights_Passengers_PassengerId",
                        column: x => x.PassengerId,
                        principalTable: "Passengers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<int>(nullable: false),
                    Row = table.Column<string>(nullable: true),
                    Number = table.Column<ushort>(nullable: false),
                    IsAvailable = table.Column<bool>(nullable: true),
                    AircraftRef = table.Column<int>(nullable: false),
                    FlightId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seats_Aircrafts_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Aircrafts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BaggageEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    FlightId = table.Column<int>(nullable: false),
                    PassengerId = table.Column<int>(nullable: false),
                    Weight = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaggageEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaggageEntity_PassengerFlights_FlightId_PassengerId",
                        columns: x => new { x.FlightId, x.PassengerId },
                        principalTable: "PassengerFlights",
                        principalColumns: new[] { "FlightId", "PassengerId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Flights",
                columns: new[] { "Id", "Created", "Depart", "Destination", "LastModified", "Origin", "Return" },
                values: new object[] { 1, new DateTime(2021, 8, 21, 20, 41, 0, 0, DateTimeKind.Utc), new DateTime(2021, 8, 22, 22, 0, 0, 0, DateTimeKind.Utc), "DC", null, "TLV", new DateTime(2021, 9, 11, 10, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Flights",
                columns: new[] { "Id", "Created", "Depart", "Destination", "LastModified", "Origin", "Return" },
                values: new object[] { 2, new DateTime(2021, 8, 21, 20, 41, 0, 0, DateTimeKind.Utc), new DateTime(2021, 8, 23, 9, 0, 0, 0, DateTimeKind.Utc), "TLV", null, "NY", new DateTime(2021, 9, 17, 22, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Passengers",
                columns: new[] { "Id", "Created", "FirtName", "LastModified", "LastName" },
                values: new object[] { 1, new DateTime(2021, 8, 21, 20, 41, 0, 0, DateTimeKind.Utc), "Avi", null, "Nessimian" });

            migrationBuilder.InsertData(
                table: "Passengers",
                columns: new[] { "Id", "Created", "FirtName", "LastModified", "LastName" },
                values: new object[] { 2, new DateTime(2021, 8, 21, 20, 41, 0, 0, DateTimeKind.Utc), "Adam", null, "Nahlaui" });

            migrationBuilder.InsertData(
                table: "Seats",
                columns: new[] { "Id", "AircraftRef", "Created", "FlightId", "IsAvailable", "LastModified", "Number", "Row", "RowVersion" },
                values: new object[] { 1, 1, new DateTime(2021, 8, 21, 20, 41, 0, 0, DateTimeKind.Utc), null, null, null, (ushort)1, "A", 1 });

            migrationBuilder.InsertData(
                table: "Seats",
                columns: new[] { "Id", "AircraftRef", "Created", "FlightId", "IsAvailable", "LastModified", "Number", "Row", "RowVersion" },
                values: new object[] { 2, 1, new DateTime(2021, 8, 21, 20, 41, 0, 0, DateTimeKind.Utc), null, null, null, (ushort)2, "A", 1 });

            migrationBuilder.InsertData(
                table: "Seats",
                columns: new[] { "Id", "AircraftRef", "Created", "FlightId", "IsAvailable", "LastModified", "Number", "Row", "RowVersion" },
                values: new object[] { 3, 1, new DateTime(2021, 8, 21, 20, 41, 0, 0, DateTimeKind.Utc), null, null, null, (ushort)1, "B", 1 });

            migrationBuilder.InsertData(
                table: "Seats",
                columns: new[] { "Id", "AircraftRef", "Created", "FlightId", "IsAvailable", "LastModified", "Number", "Row", "RowVersion" },
                values: new object[] { 4, 1, new DateTime(2021, 8, 21, 20, 41, 0, 0, DateTimeKind.Utc), null, null, null, (ushort)2, "B", 1 });

            migrationBuilder.InsertData(
                table: "Seats",
                columns: new[] { "Id", "AircraftRef", "Created", "FlightId", "IsAvailable", "LastModified", "Number", "Row", "RowVersion" },
                values: new object[] { 5, 2, new DateTime(2021, 8, 21, 20, 41, 0, 0, DateTimeKind.Utc), null, null, null, (ushort)1, "A", 1 });

            migrationBuilder.InsertData(
                table: "Seats",
                columns: new[] { "Id", "AircraftRef", "Created", "FlightId", "IsAvailable", "LastModified", "Number", "Row", "RowVersion" },
                values: new object[] { 6, 2, new DateTime(2021, 8, 21, 20, 41, 0, 0, DateTimeKind.Utc), null, null, null, (ushort)2, "A", 1 });

            migrationBuilder.InsertData(
                table: "Seats",
                columns: new[] { "Id", "AircraftRef", "Created", "FlightId", "IsAvailable", "LastModified", "Number", "Row", "RowVersion" },
                values: new object[] { 7, 2, new DateTime(2021, 8, 21, 20, 41, 0, 0, DateTimeKind.Utc), null, null, null, (ushort)1, "B", 1 });

            migrationBuilder.InsertData(
                table: "Seats",
                columns: new[] { "Id", "AircraftRef", "Created", "FlightId", "IsAvailable", "LastModified", "Number", "Row", "RowVersion" },
                values: new object[] { 8, 2, new DateTime(2021, 8, 21, 20, 41, 0, 0, DateTimeKind.Utc), null, null, null, (ushort)2, "B", 1 });

            migrationBuilder.InsertData(
                table: "Aircrafts",
                columns: new[] { "Id", "BaggagesLimit", "Created", "FlightRef", "LastModified", "WeightLimit" },
                values: new object[] { 1, (ushort)100, new DateTime(2021, 8, 21, 20, 41, 0, 0, DateTimeKind.Utc), 1, null, 1000m });

            migrationBuilder.InsertData(
                table: "Aircrafts",
                columns: new[] { "Id", "BaggagesLimit", "Created", "FlightRef", "LastModified", "WeightLimit" },
                values: new object[] { 2, (ushort)10, new DateTime(2021, 8, 21, 20, 41, 0, 0, DateTimeKind.Utc), 2, null, 100m });

            migrationBuilder.CreateIndex(
                name: "IX_Aircrafts_FlightRef",
                table: "Aircrafts",
                column: "FlightRef",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BaggageEntity_FlightId_PassengerId",
                table: "BaggageEntity",
                columns: new[] { "FlightId", "PassengerId" });

            migrationBuilder.CreateIndex(
                name: "IX_PassengerFlights_PassengerId",
                table: "PassengerFlights",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_FlightId",
                table: "Seats",
                column: "FlightId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaggageEntity");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "PassengerFlights");

            migrationBuilder.DropTable(
                name: "Aircrafts");

            migrationBuilder.DropTable(
                name: "Passengers");

            migrationBuilder.DropTable(
                name: "Flights");
        }
    }
}
