using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace smsCoffee.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class dropTableRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
          name: "Roles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a4da7d4-def7-44fe-b563-16ed22ba0fe1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e7777276-6ead-421a-8e39-605db3ee3751");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "e242ebb1-fc56-4688-9a47-fdb9670ccacb", null, "Admin", "ADMIN" },
                    { "f4843f27-b90a-44e8-b8f5-a4dc17fb3ced", null, "User", "USER" }
                });
        }
    }
}
