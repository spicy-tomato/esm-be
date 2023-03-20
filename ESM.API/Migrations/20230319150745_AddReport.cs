using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESM.API.Migrations
{
    public partial class AddReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Report",
                table: "Shifts",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "be30ee0c-af9e-4ab6-b463-937b6904105d", "AQAAAAEAACcQAAAAEGwUqhnwd1Jaxn3ab6s9OzUOftvO96Z5+jD6Zz3/VpzO3uxsYKHbaJWWD7WU+GFR2g==" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e18-c46f-4e76-8e77-69430f54d796"),
                column: "ConcurrencyStamp",
                value: "c5461e72-ea5b-4c7e-9d63-3c718b1dcd56");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e1a-7953-4790-8ebe-272e34a8fe18"),
                column: "ConcurrencyStamp",
                value: "2e30c187-3194-4cfb-adf3-303d5fb54a3f");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Report",
                table: "Shifts");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "27626065-2f65-4fab-88dc-90bb0454a896", "AQAAAAEAACcQAAAAEPZWW/q3WQxxo5ReZNqRgRmMOl+Qb7jnuNWIaB60+ry7BQEWr+wD8ifKx+OW+HRfpQ==" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e18-c46f-4e76-8e77-69430f54d796"),
                column: "ConcurrencyStamp",
                value: "717e595a-14eb-4e84-8976-c084836e0774");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e1a-7953-4790-8ebe-272e34a8fe18"),
                column: "ConcurrencyStamp",
                value: "a89075c5-8bfb-4b10-ac61-77688136296b");
        }
    }
}
