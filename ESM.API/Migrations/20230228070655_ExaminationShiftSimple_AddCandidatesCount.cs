using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESM.API.Migrations
{
    public partial class ExaminationShiftSimple_AddCandidatesCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CandidateCount",
                table: "ExaminationData",
                newName: "CandidatesCount");

            migrationBuilder.AddColumn<int>(
                name: "CandidatesCount",
                table: "ExaminationShifts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "037d8e81-f4e3-4aa7-8274-b367618500f9", "AQAAAAEAACcQAAAAEK2Hobei1j2f5ugXSggXuUcPZWL/kuPvToFpbIekwZ+TMLQviRpYrtrRRpVlVCZpgw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CandidatesCount",
                table: "ExaminationShifts");

            migrationBuilder.RenameColumn(
                name: "CandidatesCount",
                table: "ExaminationData",
                newName: "CandidateCount");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1b43a609-7ed3-41bb-b0ec-00500f5c234f", "AQAAAAEAACcQAAAAEAxgpRXCH3SCpT91cZX2LpEYFl8v3xl8Wx4NS5OQtrqsiNeCCXOSR+x+GJkaqc0z/Q==" });
        }
    }
}
