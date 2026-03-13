using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BAO_Cinemas.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLocalPostersAndAddMoreMovies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1,
                column: "PosterUrl",
                value: "/images/posters/avengers_endgame.jpg");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 2,
                column: "PosterUrl",
                value: "/images/posters/spider_man.jpg");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 3,
                column: "PosterUrl",
                value: "/images/posters/mai.jpg");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 4,
                column: "PosterUrl",
                value: "/images/posters/dune2.jpg");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 5,
                column: "PosterUrl",
                value: "/images/posters/godzilla_kong.jpg");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "PosterUrl", "Title" },
                values: new object[] { "/images/posters/avatar3.jpg", "Avatar 3" });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 7,
                column: "PosterUrl",
                value: "/images/posters/deadpool.jpg");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 8,
                column: "PosterUrl",
                value: "/images/posters/kungfu_panda4.jpg");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 9,
                column: "PosterUrl",
                value: "/images/posters/inside_out2.jpg");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CategoryId", "Duration", "Genre", "PosterUrl", "ReleaseDate", "Title" },
                values: new object[] { 2, 120, "Phiêu Lưu, Gia Đình", "/images/posters/mufasa.jpg", new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mufasa: The Lion King" });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Duration", "Genre", "PosterUrl", "ReleaseDate", "Title" },
                values: new object[] { 180, "Tiểu Sử, Lịch Sử", "/images/posters/oppenheimer.jpg", new DateTime(2023, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Oppenheimer IMAX" });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Duration", "Genre", "PosterUrl", "ReleaseDate", "Title" },
                values: new object[] { 169, "Viễn Tưởng, Không Gian", "/images/posters/interstellar.jpg", new DateTime(2014, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Interstellar IMAX" });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Duration", "Genre", "PosterUrl", "ReleaseDate", "Title" },
                values: new object[] { 152, "Hành Động, Tội Phạm", "/images/posters/dark_knight.jpg", new DateTime(2008, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Dark Knight IMAX" });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "CategoryId", "Duration", "Genre", "PosterUrl", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { 14, 3, 130, "Hành Động", "/images/posters/top_gun.jpg", new DateTime(2022, 5, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Top Gun: Maverick IMAX" },
                    { 15, 3, 148, "Hành Động, Viễn Tưởng", "/images/posters/inception.jpg", new DateTime(2010, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inception IMAX" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1,
                column: "PosterUrl",
                value: "http://googleusercontent.com/image_collection/image_retrieval/1826221526348636952");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 2,
                column: "PosterUrl",
                value: "http://googleusercontent.com/image_collection/image_retrieval/18080433966193679982");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 3,
                column: "PosterUrl",
                value: "http://googleusercontent.com/image_collection/image_retrieval/15852584358679775956");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 4,
                column: "PosterUrl",
                value: "http://googleusercontent.com/image_collection/image_retrieval/7538558842761719580");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 5,
                column: "PosterUrl",
                value: "http://googleusercontent.com/image_collection/image_retrieval/6947927671804621343");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "PosterUrl", "Title" },
                values: new object[] { "http://googleusercontent.com/image_collection/image_retrieval/5159262187925869827", "Avatar 3: Fire and Ash" });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 7,
                column: "PosterUrl",
                value: "http://googleusercontent.com/image_collection/image_retrieval/2484040348688754999");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 8,
                column: "PosterUrl",
                value: "http://googleusercontent.com/image_collection/image_retrieval/15097381491623435886");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 9,
                column: "PosterUrl",
                value: "http://googleusercontent.com/image_collection/image_retrieval/17395724413228581908");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CategoryId", "Duration", "Genre", "PosterUrl", "ReleaseDate", "Title" },
                values: new object[] { 3, 180, "Tiểu Sử, Lịch Sử", "http://googleusercontent.com/image_collection/image_retrieval/16260625346558820945", new DateTime(2023, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Oppenheimer IMAX" });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Duration", "Genre", "PosterUrl", "ReleaseDate", "Title" },
                values: new object[] { 169, "Viễn Tưởng, Không Gian", "http://googleusercontent.com/image_collection/image_retrieval/8821015231344973048", new DateTime(2014, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Interstellar IMAX" });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Duration", "Genre", "PosterUrl", "ReleaseDate", "Title" },
                values: new object[] { 152, "Hành Động, Tội Phạm", "http://googleusercontent.com/image_collection/image_retrieval/7256997866226354779", new DateTime(2008, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Dark Knight IMAX" });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Duration", "Genre", "PosterUrl", "ReleaseDate", "Title" },
                values: new object[] { 130, "Hành Động", "http://googleusercontent.com/image_collection/image_retrieval/6543075009718552229", new DateTime(2022, 5, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Top Gun: Maverick" });
        }
    }
}
