using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BAO_Cinemas.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedSeats = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPrice = table.Column<double>(type: "float", nullable: false),
                    BookingTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShowtimeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Showtimes_ShowtimeId",
                        column: x => x.ShowtimeId,
                        principalTable: "Showtimes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id-789",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f85af014-dc9a-462e-9f4c-59e18fe93069", "AQAAAAIAAYagAAAAELezB2DVLLh3I+2CPf0kQKtkKLwLdHsVRo5NHonh6wnEYRGdr77wLpqXQHxDpky5ag==", "e4b7268e-7f3b-458b-a5a8-21ef30366f79" });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ShowtimeId",
                table: "Bookings",
                column: "ShowtimeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id-789",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a8014ef5-c5dd-473b-a93e-8c4c2b7b030a", "AQAAAAIAAYagAAAAECd3zbmWZK6ZiBAsSEVCxYL53it1rxreMdGtFVgophtLbE4mse0QfeeQ+Atik0kofg==", "f55dcd5c-e83c-477f-b314-bb4e64e935ce" });
        }
    }
}
