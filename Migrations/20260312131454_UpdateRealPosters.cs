using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BAO_Cinemas.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRealPosters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                column: "PosterUrl",
                value: "http://googleusercontent.com/image_collection/image_retrieval/16260625346558820945");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 11,
                column: "PosterUrl",
                value: "http://googleusercontent.com/image_collection/image_retrieval/8821015231344973048");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 12,
                column: "PosterUrl",
                value: "http://googleusercontent.com/image_collection/image_retrieval/7256997866226354779");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 13,
                column: "PosterUrl",
                value: "http://googleusercontent.com/image_collection/image_retrieval/6543075009718552229");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1,
                column: "PosterUrl",
                value: "https://placehold.co/300x450/1a1a1a/FFF?text=Avengers");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 2,
                column: "PosterUrl",
                value: "https://placehold.co/300x450/1a1a1a/FFF?text=Spider-Man");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 3,
                column: "PosterUrl",
                value: "https://placehold.co/300x450/1a1a1a/FFF?text=Mai");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 4,
                column: "PosterUrl",
                value: "https://placehold.co/300x450/1a1a1a/FFF?text=Dune+2");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 5,
                column: "PosterUrl",
                value: "https://placehold.co/300x450/1a1a1a/FFF?text=GxK");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "PosterUrl", "Title" },
                values: new object[] { "https://placehold.co/300x450/1a1a1a/FFF?text=Avatar+3", "Avatar 3" });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 7,
                column: "PosterUrl",
                value: "https://placehold.co/300x450/1a1a1a/FFF?text=Deadpool");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 8,
                column: "PosterUrl",
                value: "https://placehold.co/300x450/1a1a1a/FFF?text=Kung+Fu");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 9,
                column: "PosterUrl",
                value: "https://placehold.co/300x450/1a1a1a/FFF?text=Inside+Out");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 10,
                column: "PosterUrl",
                value: "https://placehold.co/300x450/1a1a1a/FFF?text=Oppenheimer");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 11,
                column: "PosterUrl",
                value: "https://placehold.co/300x450/1a1a1a/FFF?text=Interstellar");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 12,
                column: "PosterUrl",
                value: "https://placehold.co/300x450/1a1a1a/FFF?text=Dark+Knight");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 13,
                column: "PosterUrl",
                value: "https://placehold.co/300x450/1a1a1a/FFF?text=Top+Gun");
        }
    }
}
