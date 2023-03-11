using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESM.API.Migrations
{
    public partial class AddFacultyShiftGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentShiftGroup_FacultyShiftGroup_FacultyShiftGroupId",
                table: "DepartmentShiftGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_FacultyShiftGroup_Faculties_FacultyId",
                table: "FacultyShiftGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_FacultyShiftGroup_ShiftGroups_ShiftGroupId",
                table: "FacultyShiftGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FacultyShiftGroup",
                table: "FacultyShiftGroup");

            migrationBuilder.RenameTable(
                name: "FacultyShiftGroup",
                newName: "FacultyShiftGroups");

            migrationBuilder.RenameIndex(
                name: "IX_FacultyShiftGroup_ShiftGroupId",
                table: "FacultyShiftGroups",
                newName: "IX_FacultyShiftGroups_ShiftGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_FacultyShiftGroup_FacultyId",
                table: "FacultyShiftGroups",
                newName: "IX_FacultyShiftGroups_FacultyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FacultyShiftGroups",
                table: "FacultyShiftGroups",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentShiftGroup_FacultyShiftGroups_FacultyShiftGroupId",
                table: "DepartmentShiftGroup",
                column: "FacultyShiftGroupId",
                principalTable: "FacultyShiftGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FacultyShiftGroups_Faculties_FacultyId",
                table: "FacultyShiftGroups",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FacultyShiftGroups_ShiftGroups_ShiftGroupId",
                table: "FacultyShiftGroups",
                column: "ShiftGroupId",
                principalTable: "ShiftGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentShiftGroup_FacultyShiftGroups_FacultyShiftGroupId",
                table: "DepartmentShiftGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_FacultyShiftGroups_Faculties_FacultyId",
                table: "FacultyShiftGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_FacultyShiftGroups_ShiftGroups_ShiftGroupId",
                table: "FacultyShiftGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FacultyShiftGroups",
                table: "FacultyShiftGroups");

            migrationBuilder.RenameTable(
                name: "FacultyShiftGroups",
                newName: "FacultyShiftGroup");

            migrationBuilder.RenameIndex(
                name: "IX_FacultyShiftGroups_ShiftGroupId",
                table: "FacultyShiftGroup",
                newName: "IX_FacultyShiftGroup_ShiftGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_FacultyShiftGroups_FacultyId",
                table: "FacultyShiftGroup",
                newName: "IX_FacultyShiftGroup_FacultyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FacultyShiftGroup",
                table: "FacultyShiftGroup",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "033b2266-67a5-4a95-ae7b-e4308cbe9089", "AQAAAAEAACcQAAAAEDlSB7u1c5JS9TDjJgTjJq2fjGdPEg4a+5/jNWiEv1VZb/ri2cFqpHmixWw38MNU/g==" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e18-c46f-4e76-8e77-69430f54d796"),
                column: "ConcurrencyStamp",
                value: "7e8f03f8-8b30-4afc-bf8a-6abc8f464225");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e1a-7953-4790-8ebe-272e34a8fe18"),
                column: "ConcurrencyStamp",
                value: "c18b68d4-37f6-4f1b-b74c-fea043f43157");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentShiftGroup_FacultyShiftGroup_FacultyShiftGroupId",
                table: "DepartmentShiftGroup",
                column: "FacultyShiftGroupId",
                principalTable: "FacultyShiftGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FacultyShiftGroup_Faculties_FacultyId",
                table: "FacultyShiftGroup",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FacultyShiftGroup_ShiftGroups_ShiftGroupId",
                table: "FacultyShiftGroup",
                column: "ShiftGroupId",
                principalTable: "ShiftGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
