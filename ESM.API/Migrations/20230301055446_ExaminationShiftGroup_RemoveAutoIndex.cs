using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESM.API.Migrations
{
    public partial class ExaminationShiftGroup_RemoveAutoIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3d2dc4ec-e733-4992-8a06-089edd20b556", "AQAAAAEAACcQAAAAELT3rFeQGlxhqSEHLcvzc1N/uxQQcSj2TqouT5lfCXTolWKjt/0tCCA0Wt0JMQ57ZQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "42cebf77-26e5-4a78-ab1e-e19208893f0c", "AQAAAAEAACcQAAAAEAJLCrcivLmERDkLp1bXgeB77iI+9vqcokheK0BOHrpfvBcMhV8xsyv4fmGsShnsTg==" });
        }
    }
}
