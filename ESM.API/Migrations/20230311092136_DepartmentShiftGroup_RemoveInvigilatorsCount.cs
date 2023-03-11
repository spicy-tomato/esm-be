using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESM.API.Migrations
{
    public partial class DepartmentShiftGroup_RemoveInvigilatorsCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentShiftGroups_Departments_DepartmentId",
                table: "DepartmentShiftGroups");

            migrationBuilder.DropColumn(
                name: "InvigilatorsCount",
                table: "DepartmentShiftGroups");

            migrationBuilder.AlterColumn<Guid>(
                name: "DepartmentId",
                table: "DepartmentShiftGroups",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "8e561f4f-5d33-4fbb-9fd1-838646ff61e6", "AQAAAAEAACcQAAAAEJB64JV9rkRsMrR+BVLq8GX1bydA6+4DkyhU335E7oK2TQ+tF5NuXjDqJX4ijYfS+A==" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e18-c46f-4e76-8e77-69430f54d796"),
                column: "ConcurrencyStamp",
                value: "def3c198-02c6-4eb7-80db-0070e85fda6c");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e1a-7953-4790-8ebe-272e34a8fe18"),
                column: "ConcurrencyStamp",
                value: "19bebb2f-26d9-4841-a391-f5d304ebef6c");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentShiftGroups_Departments_DepartmentId",
                table: "DepartmentShiftGroups",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentShiftGroups_Departments_DepartmentId",
                table: "DepartmentShiftGroups");

            migrationBuilder.AlterColumn<Guid>(
                name: "DepartmentId",
                table: "DepartmentShiftGroups",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddColumn<int>(
                name: "InvigilatorsCount",
                table: "DepartmentShiftGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ff4ef234-a958-4052-be40-335ba1f75e95", "AQAAAAEAACcQAAAAEEdXz9qh162sjDREMKF2KtY52PSc01Jfxczia/04PlKjWGTDyeRfwht1Z49AKVns6w==" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e18-c46f-4e76-8e77-69430f54d796"),
                column: "ConcurrencyStamp",
                value: "ee8ffb9c-d7ca-40bf-b16d-e0e1a2b1e277");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e1a-7953-4790-8ebe-272e34a8fe18"),
                column: "ConcurrencyStamp",
                value: "fb54aab8-5c05-4e87-a09d-55f2b6b53944");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentShiftGroups_Departments_DepartmentId",
                table: "DepartmentShiftGroups",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
