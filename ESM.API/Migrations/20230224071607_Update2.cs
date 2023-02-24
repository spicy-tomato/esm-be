using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESM.API.Migrations
{
    public partial class Update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Invigilator_InvigilatorId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_InvigilatorExaminationModule_Invigilator_InvigilatorId",
                table: "InvigilatorExaminationModule");

            migrationBuilder.DropForeignKey(
                name: "FK_InvigilatorShift_Invigilator_InvigilatorId",
                table: "InvigilatorShift");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invigilator",
                table: "Invigilator");

            migrationBuilder.RenameTable(
                name: "Invigilator",
                newName: "Invigilators");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invigilators",
                table: "Invigilators",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4261896a-5d86-4695-a220-018ba97cabc1", "AQAAAAEAACcQAAAAEGRSVgNPGUdaezxBmX13ZOHBGib9v5z1TAHQprG8cmEaC9yxjDdIezBEu33mk0i+Dw==" });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Invigilators_InvigilatorId",
                table: "AspNetUsers",
                column: "InvigilatorId",
                principalTable: "Invigilators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvigilatorExaminationModule_Invigilators_InvigilatorId",
                table: "InvigilatorExaminationModule",
                column: "InvigilatorId",
                principalTable: "Invigilators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvigilatorShift_Invigilators_InvigilatorId",
                table: "InvigilatorShift",
                column: "InvigilatorId",
                principalTable: "Invigilators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Invigilators_InvigilatorId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_InvigilatorExaminationModule_Invigilators_InvigilatorId",
                table: "InvigilatorExaminationModule");

            migrationBuilder.DropForeignKey(
                name: "FK_InvigilatorShift_Invigilators_InvigilatorId",
                table: "InvigilatorShift");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invigilators",
                table: "Invigilators");

            migrationBuilder.RenameTable(
                name: "Invigilators",
                newName: "Invigilator");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invigilator",
                table: "Invigilator",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "28595e70-a439-4444-bcda-68da06f1090f", "AQAAAAEAACcQAAAAEKERqcWS4rNSzDp7hjbmg/njv6UeWU0IYz/+u6FBDKhcz2kAlXBwLYLwpycVSx0/6A==" });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Invigilator_InvigilatorId",
                table: "AspNetUsers",
                column: "InvigilatorId",
                principalTable: "Invigilator",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvigilatorExaminationModule_Invigilator_InvigilatorId",
                table: "InvigilatorExaminationModule",
                column: "InvigilatorId",
                principalTable: "Invigilator",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvigilatorShift_Invigilator_InvigilatorId",
                table: "InvigilatorShift",
                column: "InvigilatorId",
                principalTable: "Invigilator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
