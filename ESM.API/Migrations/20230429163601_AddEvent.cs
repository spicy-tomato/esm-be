using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESM.API.Migrations
{
    public partial class AddEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExaminationEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ExaminationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExaminationEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExaminationEvents_Examinations_ExaminationId",
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
                values: new object[] { "68217d9d-9f61-41fb-9b66-e8a643522866", "AQAAAAEAACcQAAAAELl/J7ORdJQ7+mp38aQCuEcPC8dzT/4D7mn4raVcKT1RKpE00t2frjEwfWrYcA8bHQ==" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e18-c46f-4e76-8e77-69430f54d796"),
                column: "ConcurrencyStamp",
                value: "add7624f-f63b-4d48-8cb5-a51a19a9dd12");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e1a-7953-4790-8ebe-272e34a8fe18"),
                column: "ConcurrencyStamp",
                value: "41cb3ca3-869a-4d9a-aaf2-a83335593820");

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationEvents_ExaminationId",
                table: "ExaminationEvents",
                column: "ExaminationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExaminationEvents");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "be30ee0c-af9e-4ab6-b463-937b6904105d", "AQAAAAEAACcQAAAAEGwUqhnwd1Jaxn3ab6s9OzUOftvO96Z5+jD6Zz3/VpzO3uxsYKHbaJWWD7WU+GFR2g==" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e18-c46f-4e76-8e77-69430f54d796"),
                column: "ConcurrencyStamp",
                value: "c5461e72-ea5b-4c7e-9d63-3c718b1dcd56");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e1a-7953-4790-8ebe-272e34a8fe18"),
                column: "ConcurrencyStamp",
                value: "2e30c187-3194-4cfb-adf3-303d5fb54a3f");
        }
    }
}
