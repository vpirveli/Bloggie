using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bloggie.Web.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class UpdateminorMistake : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7858c7ff-c709-4294-8248-1c9fa186f718",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "SuperAdmin", "SuperAdmin" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "786c67eb-f7ed-4b99-b7f4-57009ba2479e",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "User", "User" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "82db149f-c32f-4271-af58-0df7ffd91dfc",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Admin", "Admin" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "685d2d9c-957e-429d-b86c-c7cf79ef4dfb",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "888d3ea8-d708-4b39-8d81-dfa2360bfe63", "AQAAAAIAAYagAAAAEP/GA5swbJTi5U3fsIYCgckCrkRX3KrKinSTUkUiRSH0nFN1hJy0POQRO8SgwnBuxg==", "604a1546-4c48-4b6b-8f48-fc9d7ca3ad3c" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7858c7ff-c709-4294-8248-1c9fa186f718",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "superAdmin", "superAdmin" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "786c67eb-f7ed-4b99-b7f4-57009ba2479e",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "user", "user" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "82db149f-c32f-4271-af58-0df7ffd91dfc",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "admin", "admin" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "685d2d9c-957e-429d-b86c-c7cf79ef4dfb",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "71f0bf74-09ea-47f3-9900-fad49288f241", "AQAAAAIAAYagAAAAEK/2LJPJSbLXeqbWeYB4DZxoUMulggxWroeVtRDPbRoe5Gi4oUvQKz8MFlNJzKPUyQ==", "9d82de01-85c8-4927-9ecc-42fe96e90c16" });
        }
    }
}
