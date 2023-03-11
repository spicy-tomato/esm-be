using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESM.API.Migrations
{
    public partial class Add_DepartmentShiftGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentShiftGroup_AspNetUsers_UserId",
                table: "DepartmentShiftGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentShiftGroup_Departments_DepartmentId",
                table: "DepartmentShiftGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentShiftGroup_FacultyShiftGroups_FacultyShiftGroupId",
                table: "DepartmentShiftGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepartmentShiftGroup",
                table: "DepartmentShiftGroup");

            migrationBuilder.RenameTable(
                name: "DepartmentShiftGroup",
                newName: "DepartmentShiftGroups");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentShiftGroup_UserId",
                table: "DepartmentShiftGroups",
                newName: "IX_DepartmentShiftGroups_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentShiftGroup_FacultyShiftGroupId",
                table: "DepartmentShiftGroups",
                newName: "IX_DepartmentShiftGroups_FacultyShiftGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentShiftGroup_DepartmentId",
                table: "DepartmentShiftGroups",
                newName: "IX_DepartmentShiftGroups_DepartmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepartmentShiftGroups",
                table: "DepartmentShiftGroups",
                column: "Id");

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
                name: "FK_DepartmentShiftGroups_AspNetUsers_UserId",
                table: "DepartmentShiftGroups",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentShiftGroups_Departments_DepartmentId",
                table: "DepartmentShiftGroups",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentShiftGroups_FacultyShiftGroups_FacultyShiftGroupId",
                table: "DepartmentShiftGroups",
                column: "FacultyShiftGroupId",
                principalTable: "FacultyShiftGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentShiftGroups_AspNetUsers_UserId",
                table: "DepartmentShiftGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentShiftGroups_Departments_DepartmentId",
                table: "DepartmentShiftGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentShiftGroups_FacultyShiftGroups_FacultyShiftGroupId",
                table: "DepartmentShiftGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepartmentShiftGroups",
                table: "DepartmentShiftGroups");

            migrationBuilder.RenameTable(
                name: "DepartmentShiftGroups",
                newName: "DepartmentShiftGroup");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentShiftGroups_UserId",
                table: "DepartmentShiftGroup",
                newName: "IX_DepartmentShiftGroup_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentShiftGroups_FacultyShiftGroupId",
                table: "DepartmentShiftGroup",
                newName: "IX_DepartmentShiftGroup_FacultyShiftGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentShiftGroups_DepartmentId",
                table: "DepartmentShiftGroup",
                newName: "IX_DepartmentShiftGroup_DepartmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepartmentShiftGroup",
                table: "DepartmentShiftGroup",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentShiftGroup_AspNetUsers_UserId",
                table: "DepartmentShiftGroup",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentShiftGroup_Departments_DepartmentId",
                table: "DepartmentShiftGroup",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentShiftGroup_FacultyShiftGroups_FacultyShiftGroupId",
                table: "DepartmentShiftGroup",
                column: "FacultyShiftGroupId",
                principalTable: "FacultyShiftGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
