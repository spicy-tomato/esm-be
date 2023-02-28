using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESM.API.Migrations
{
    public partial class ExaminationShiftSimple_AddDepartmentAssign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                values: new object[] { "34791bb8-fbf5-4383-a656-3c1cc407d93b", "AQAAAAEAACcQAAAAEGbvhy8DGkA5qv7w1djNEwbgPVvDiPjkVfFkpS67/avX1XhXwZ9slfyo40cF5hTqgA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentAssign",
                table: "ExaminationShifts");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "037d8e81-f4e3-4aa7-8274-b367618500f9", "AQAAAAEAACcQAAAAEK2Hobei1j2f5ugXSggXuUcPZWL/kuPvToFpbIekwZ+TMLQviRpYrtrRRpVlVCZpgw==" });
        }
    }
}
