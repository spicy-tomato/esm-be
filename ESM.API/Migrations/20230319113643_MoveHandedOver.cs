using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESM.API.Migrations
{
    public partial class MoveHandedOver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HandedOver",
                table: "InvigilatorShift");

            migrationBuilder.AddColumn<Guid>(
                name: "HandedOverUserId",
                table: "Shifts",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

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

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_HandedOverUserId",
                table: "Shifts",
                column: "HandedOverUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_AspNetUsers_HandedOverUserId",
                table: "Shifts",
                column: "HandedOverUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_AspNetUsers_HandedOverUserId",
                table: "Shifts");

            migrationBuilder.DropIndex(
                name: "IX_Shifts_HandedOverUserId",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "HandedOverUserId",
                table: "Shifts");

            migrationBuilder.AddColumn<bool>(
                name: "HandedOver",
                table: "InvigilatorShift",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b13e36e0-c822-458f-97c6-919a8247960b", "AQAAAAEAACcQAAAAEBvf8nNGawNY1HoqgGUPNh95xdf5SojDy3HcmFmv6UkO2Y68uxkzzKN2rQQa12HmpQ==" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e18-c46f-4e76-8e77-69430f54d796"),
                column: "ConcurrencyStamp",
                value: "3f86478c-7204-494f-b5a1-325feeb64835");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e1a-7953-4790-8ebe-272e34a8fe18"),
                column: "ConcurrencyStamp",
                value: "b6bdc5a7-6ee0-4825-ad17-741111771bbc");
        }
    }
}
