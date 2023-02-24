using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESM.API.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvigilatorExaminationModule_Invigilator_InvigilatorId",
                table: "InvigilatorExaminationModule");

            migrationBuilder.AlterColumn<Guid>(
                name: "InvigilatorId",
                table: "InvigilatorExaminationModule",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "TemporaryInvigilatorName",
                table: "InvigilatorExaminationModule",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "28595e70-a439-4444-bcda-68da06f1090f", "AQAAAAEAACcQAAAAEKERqcWS4rNSzDp7hjbmg/njv6UeWU0IYz/+u6FBDKhcz2kAlXBwLYLwpycVSx0/6A==" });

            migrationBuilder.AddForeignKey(
                name: "FK_InvigilatorExaminationModule_Invigilator_InvigilatorId",
                table: "InvigilatorExaminationModule",
                column: "InvigilatorId",
                principalTable: "Invigilator",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvigilatorExaminationModule_Invigilator_InvigilatorId",
                table: "InvigilatorExaminationModule");

            migrationBuilder.DropColumn(
                name: "TemporaryInvigilatorName",
                table: "InvigilatorExaminationModule");

            migrationBuilder.AlterColumn<Guid>(
                name: "InvigilatorId",
                table: "InvigilatorExaminationModule",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "639d4316-2a22-41d4-ac28-1ccd652d94dd", "AQAAAAEAACcQAAAAEAvJn/wAL443ZPSWVUt6a5XiItmAtre03Ul8Wz0pasB4DDrF1ysfgpBu0TNOZTXUQg==" });

            migrationBuilder.AddForeignKey(
                name: "FK_InvigilatorExaminationModule_Invigilator_InvigilatorId",
                table: "InvigilatorExaminationModule",
                column: "InvigilatorId",
                principalTable: "Invigilator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
