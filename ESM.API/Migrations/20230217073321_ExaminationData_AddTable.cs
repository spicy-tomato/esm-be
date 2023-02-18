using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESM.API.Migrations
{
    public partial class ExaminationData_AddTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExaminationData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ModuleId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModuleName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModuleClass = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Credit = table.Column<int>(type: "int", nullable: true),
                    Method = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    StartAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EndAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Shift = table.Column<int>(type: "int", nullable: true),
                    CandidateCount = table.Column<int>(type: "int", nullable: true),
                    RoomsCount = table.Column<int>(type: "int", nullable: true),
                    Rooms = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Faculty = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Department = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DepartmentAssign = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    ExaminationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExaminationData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExaminationData_Examinations_ExaminationId",
                        column: x => x.ExaminationId,
                        principalTable: "Examinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ff7baf09-a790-41c2-8734-20e49cdb2fc3", "AQAAAAEAACcQAAAAEPEiMK0qo+W/4bUNpMpJY1SjofHYQP59uNkmrWe95H8mCUjVojjl0e+BMsDh0L7JJQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationData_ExaminationId",
                table: "ExaminationData",
                column: "ExaminationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExaminationData");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "203cb277-8338-4df3-b631-664b706b97d3", "AQAAAAEAACcQAAAAEOtZVfL0Pk95d39p7ooZMDh2Ftjt+FABSjBzAGV5RBuRfDEh9dK0mFL4/QsI0L0LpQ==" });
        }
    }
}
