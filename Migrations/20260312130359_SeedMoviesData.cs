using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BAO_Cinemas.Migrations
{
    /// <inheritdoc />
    public partial class SeedMoviesData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "CategoryId", "Duration", "Genre", "PosterUrl", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { 1, 1, 180, "Hành Động, Viễn Tưởng", "https://placehold.co/300x450/1a1a1a/FFF?text=Avengers", new DateTime(2019, 4, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Avengers: Endgame" },
                    { 2, 1, 148, "Hành Động", "https://placehold.co/300x450/1a1a1a/FFF?text=Spider-Man", new DateTime(2021, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spider-Man: No Way Home" },
                    { 3, 1, 131, "Tâm Lý, Tình Cảm", "https://placehold.co/300x450/1a1a1a/FFF?text=Mai", new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mai" },
                    { 4, 1, 166, "Viễn Tưởng, Hành Động", "https://placehold.co/300x450/1a1a1a/FFF?text=Dune+2", new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dune: Part Two" },
                    { 5, 1, 115, "Hành Động, Quái Vật", "https://placehold.co/300x450/1a1a1a/FFF?text=GxK", new DateTime(2024, 3, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Godzilla x Kong" },
                    { 6, 2, 190, "Viễn Tưởng, Phiêu Lưu", "https://placehold.co/300x450/1a1a1a/FFF?text=Avatar+3", new DateTime(2025, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Avatar 3" },
                    { 7, 2, 127, "Hành Động, Hài", "https://placehold.co/300x450/1a1a1a/FFF?text=Deadpool", new DateTime(2024, 7, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deadpool & Wolverine" },
                    { 8, 2, 94, "Hoạt Hình, Hài", "https://placehold.co/300x450/1a1a1a/FFF?text=Kung+Fu", new DateTime(2024, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kung Fu Panda 4" },
                    { 9, 2, 100, "Hoạt Hình, Tâm Lý", "https://placehold.co/300x450/1a1a1a/FFF?text=Inside+Out", new DateTime(2024, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inside Out 2" },
                    { 10, 3, 180, "Tiểu Sử, Lịch Sử", "https://placehold.co/300x450/1a1a1a/FFF?text=Oppenheimer", new DateTime(2023, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Oppenheimer IMAX" },
                    { 11, 3, 169, "Viễn Tưởng, Không Gian", "https://placehold.co/300x450/1a1a1a/FFF?text=Interstellar", new DateTime(2014, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Interstellar IMAX" },
                    { 12, 3, 152, "Hành Động, Tội Phạm", "https://placehold.co/300x450/1a1a1a/FFF?text=Dark+Knight", new DateTime(2008, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Dark Knight IMAX" },
                    { 13, 3, 130, "Hành Động", "https://placehold.co/300x450/1a1a1a/FFF?text=Top+Gun", new DateTime(2022, 5, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Top Gun: Maverick" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 13);
        }
    }
}
