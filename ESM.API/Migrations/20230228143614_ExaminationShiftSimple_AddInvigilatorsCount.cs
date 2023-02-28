using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESM.API.Migrations
{
    public partial class ExaminationShiftSimple_AddInvigilatorsCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvigilatorsCount",
                table: "ExaminationShifts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "81ec7d6c-12a9-4657-89cb-ef3c0b560797", "AQAAAAEAACcQAAAAEK1nOvybm5H6vr2bQD9ska5mqxGnOQkc8LEZka5uMSZ70xSLNsabioS8DZcgm2yhgg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvigilatorsCount",
                table: "ExaminationShifts");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "34791bb8-fbf5-4383-a656-3c1cc407d93b", "AQAAAAEAACcQAAAAEGbvhy8DGkA5qv7w1djNEwbgPVvDiPjkVfFkpS67/avX1XhXwZ9slfyo40cF5hTqgA==" });
        }
    }
}
