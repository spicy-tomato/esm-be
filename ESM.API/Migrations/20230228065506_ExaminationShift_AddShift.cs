using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESM.API.Migrations
{
    public partial class ExaminationShift_AddShift : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Shift",
                table: "ExaminationShifts",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1b43a609-7ed3-41bb-b0ec-00500f5c234f", "AQAAAAEAACcQAAAAEAxgpRXCH3SCpT91cZX2LpEYFl8v3xl8Wx4NS5OQtrqsiNeCCXOSR+x+GJkaqc0z/Q==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Shift",
                table: "ExaminationShifts");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "74f3f91a-1aae-4bff-8b69-c0f7e27ac54c", "AQAAAAEAACcQAAAAEFJtuyHa3rB4gXItRkc4H4QKgIzTodmwlnKVFmvP8tV/fi2uoEU2O5ubPCRe1tYNAg==" });
        }
    }
}
