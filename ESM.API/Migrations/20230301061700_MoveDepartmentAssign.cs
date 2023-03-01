using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESM.API.Migrations
{
    public partial class MoveDepartmentAssign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentAssign",
                table: "ExaminationShifts");

            migrationBuilder.AddColumn<bool>(
                name: "DepartmentAssign",
                table: "ExaminationShiftGroups",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "8a8e787b-5839-4be4-8c76-ac66a999f8c5", "AQAAAAEAACcQAAAAENiSzlfQ1xZOeTIDMVKM61PecrJf81pen9VcL4X5Y0Z7Sdex4Tw6il0dAunEzFOvtA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentAssign",
                table: "ExaminationShiftGroups");

            migrationBuilder.AddColumn<bool>(
                name: "DepartmentAssign",
                table: "ExaminationShifts",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "754176be-b464-4eff-a507-3f39dbdce688", "AQAAAAEAACcQAAAAEATyxwvRpENZNz+UjaecE+k8VxvpkqPtBrFCMz9VNcfE1YjHigPRg91sGKY39hCP/A==" });
        }
    }
}
