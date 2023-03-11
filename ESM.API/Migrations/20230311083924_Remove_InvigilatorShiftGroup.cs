using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESM.API.Migrations
{
    public partial class Remove_InvigilatorShiftGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvigilatorShiftGroup");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "DepartmentShiftGroup",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f1410462-b63e-494f-a0e5-f55db89ec181", "AQAAAAEAACcQAAAAEJgyyEmjwNs2pTKpYwfuv+33wAW5/ev3nS6DU24DhqTKNYIMxdpwrvmXzEKLloPang==" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e18-c46f-4e76-8e77-69430f54d796"),
                column: "ConcurrencyStamp",
                value: "1747b952-6626-4f80-b971-243f4396f725");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e1a-7953-4790-8ebe-272e34a8fe18"),
                column: "ConcurrencyStamp",
                value: "7f76d635-6db0-4475-8026-bf2ec499974b");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentShiftGroup_UserId",
                table: "DepartmentShiftGroup",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentShiftGroup_AspNetUsers_UserId",
                table: "DepartmentShiftGroup",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentShiftGroup_AspNetUsers_UserId",
                table: "DepartmentShiftGroup");

            migrationBuilder.DropIndex(
                name: "IX_DepartmentShiftGroup_UserId",
                table: "DepartmentShiftGroup");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DepartmentShiftGroup");

            migrationBuilder.CreateTable(
                name: "InvigilatorShiftGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DepartmentShiftGroupId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvigilatorShiftGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvigilatorShiftGroup_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvigilatorShiftGroup_DepartmentShiftGroup_DepartmentShiftGr~",
                        column: x => x.DepartmentShiftGroupId,
                        principalTable: "DepartmentShiftGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e38e4718-ff28-49c1-aed1-ccc22d5eea46", "AQAAAAEAACcQAAAAEAVvw59rjbDCNlw4Q/QOM4TEAZr26AP9OdvN4c3rthZQWWkujlCVFKfc9I+XpPWpNw==" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e18-c46f-4e76-8e77-69430f54d796"),
                column: "ConcurrencyStamp",
                value: "e5a84466-9d2f-47df-b213-00f4fa2d66fe");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e1a-7953-4790-8ebe-272e34a8fe18"),
                column: "ConcurrencyStamp",
                value: "8bdf29c8-6a18-4678-b649-6c39bdbe9354");

            migrationBuilder.CreateIndex(
                name: "IX_InvigilatorShiftGroup_DepartmentShiftGroupId",
                table: "InvigilatorShiftGroup",
                column: "DepartmentShiftGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_InvigilatorShiftGroup_UserId",
                table: "InvigilatorShiftGroup",
                column: "UserId");
        }
    }
}
