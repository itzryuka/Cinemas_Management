using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BAO_Cinemas.Migrations
{
    /// <inheritdoc />
    public partial class InitBookingSystemFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cinemas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hotline = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cinemas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalSeats = table.Column<int>(type: "int", nullable: false),
                    CinemaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Cinemas_CinemaId",
                        column: x => x.CinemaId,
                        principalTable: "Cinemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Showtimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    BookedSeats = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    CinemaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Showtimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Showtimes_Cinemas_CinemaId",
                        column: x => x.CinemaId,
                        principalTable: "Cinemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Showtimes_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Showtimes_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Cinemas",
                columns: new[] { "Id", "Address", "Hotline", "Name" },
                values: new object[,]
                {
                    { 1, "241 Xuân Thủy, Cầu Giấy, Hà Nội", "1900 1111", "BAO Cinemas Cầu Giấy" },
                    { 2, "Bitexco, Quận 1, TP.HCM", "1900 2222", "BAO Cinemas Quận 1" }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "CinemaId", "Name", "TotalSeats" },
                values: new object[,]
                {
                    { 1, 1, "Phòng 1 - Standard", 98 },
                    { 2, 1, "Phòng 2 - IMAX", 98 },
                    { 3, 2, "Phòng 1 - Standard", 98 }
                });

            migrationBuilder.InsertData(
                table: "Showtimes",
                columns: new[] { "Id", "BookedSeats", "CinemaId", "EndTime", "MovieId", "Price", "RoomId", "StartTime" },
                values: new object[,]
                {
                    { 1, "E7,E8", 1, new DateTime(2026, 3, 13, 13, 30, 0, 0, DateTimeKind.Unspecified), 1, 75000.0, 1, new DateTime(2026, 3, 13, 10, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "", 1, new DateTime(2026, 3, 13, 17, 0, 0, 0, DateTimeKind.Unspecified), 1, 90000.0, 2, new DateTime(2026, 3, 13, 14, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "A1,A2,A3", 2, new DateTime(2026, 3, 13, 22, 45, 0, 0, DateTimeKind.Unspecified), 1, 100000.0, 3, new DateTime(2026, 3, 13, 19, 45, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "", 2, new DateTime(2026, 3, 14, 12, 0, 0, 0, DateTimeKind.Unspecified), 1, 75000.0, 3, new DateTime(2026, 3, 14, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "G13,G14", 1, new DateTime(2026, 3, 13, 17, 30, 0, 0, DateTimeKind.Unspecified), 2, 80000.0, 1, new DateTime(2026, 3, 13, 15, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "D5,D6,D7", 1, new DateTime(2026, 3, 14, 22, 10, 0, 0, DateTimeKind.Unspecified), 3, 85000.0, 1, new DateTime(2026, 3, 14, 20, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_CinemaId",
                table: "Rooms",
                column: "CinemaId");

            migrationBuilder.CreateIndex(
                name: "IX_Showtimes_CinemaId",
                table: "Showtimes",
                column: "CinemaId");

            migrationBuilder.CreateIndex(
                name: "IX_Showtimes_MovieId",
                table: "Showtimes",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Showtimes_RoomId",
                table: "Showtimes",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Showtimes");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Cinemas");
        }
    }
}
