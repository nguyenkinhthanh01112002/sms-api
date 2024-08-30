using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace smsCoffee.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class seedingroles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b1e69615-20ee-49d0-b8c1-4f1d4276a8fc", null, "User", "USER" },
                    { "b75f0a43-23b5-483c-a5b9-6c138e01a7cf", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b1e69615-20ee-49d0-b8c1-4f1d4276a8fc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b75f0a43-23b5-483c-a5b9-6c138e01a7cf");
        }
    }
}
