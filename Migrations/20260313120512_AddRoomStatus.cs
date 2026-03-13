using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BAO_Cinemas.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id-789",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a8014ef5-c5dd-473b-a93e-8c4c2b7b030a", "AQAAAAIAAYagAAAAECd3zbmWZK6ZiBAsSEVCxYL53it1rxreMdGtFVgophtLbE4mse0QfeeQ+Atik0kofg==", "f55dcd5c-e83c-477f-b314-bb4e64e935ce" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 3,
                column: "Status",
                value: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Rooms");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id-789",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bbce8c4a-dd9c-4ae9-b9ac-ae5810c8f6f5", "AQAAAAIAAYagAAAAEEeeEQ0beGSeN9Qw0QpIzFvmdfCDSTfIO4davREvnZXkWD17kjKEcVE+JVZjHGL9VQ==", "806d90c7-bfe8-453c-8c77-b7b2ee294fdf" });
        }
    }
}
