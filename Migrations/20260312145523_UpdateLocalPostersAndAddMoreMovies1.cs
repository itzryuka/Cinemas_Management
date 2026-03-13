using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BAO_Cinemas.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLocalPostersAndAddMoreMovies1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Genre", "PosterUrl", "ReleaseDate", "Title" },
                values: new object[] { "Kinh Dị", "/images/posters/chao_vu_nha.jpg", new DateTime(2025, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chào Vũ Nhá" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Genre", "PosterUrl", "ReleaseDate", "Title" },
                values: new object[] { "Hành Động", "/images/posters/spider_man.jpg", new DateTime(2021, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spider-Man: No Way Home" });
        }
    }
}
