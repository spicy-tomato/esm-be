using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESM.API.Migrations
{
    public partial class Module_AddName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CandidateExaminationModule_Module_ModuleId",
                table: "CandidateExaminationModule");

            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationShift_Module_ModuleId",
                table: "ExaminationShift");

            migrationBuilder.DropForeignKey(
                name: "FK_InvigilatorExaminationModule_Module_ModuleId",
                table: "InvigilatorExaminationModule");

            migrationBuilder.DropForeignKey(
                name: "FK_Module_Faculties_FacultyId",
                table: "Module");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Module",
                table: "Module");

            migrationBuilder.RenameTable(
                name: "Module",
                newName: "Modules");

            migrationBuilder.RenameIndex(
                name: "IX_Module_FacultyId",
                table: "Modules",
                newName: "IX_Modules_FacultyId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Modules",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Modules",
                table: "Modules",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c099dd61-b6ae-483d-9d8e-4b68520938ce", "AQAAAAEAACcQAAAAEKP08Xs4Qo15TI7EbRO1pllw9e/Daz2ijjJogeDoIbdVXd0zabGBd+Yn5X1wKJouOA==" });

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateExaminationModule_Modules_ModuleId",
                table: "CandidateExaminationModule",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationShift_Modules_ModuleId",
                table: "ExaminationShift",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvigilatorExaminationModule_Modules_ModuleId",
                table: "InvigilatorExaminationModule",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Faculties_FacultyId",
                table: "Modules",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CandidateExaminationModule_Modules_ModuleId",
                table: "CandidateExaminationModule");

            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationShift_Modules_ModuleId",
                table: "ExaminationShift");

            migrationBuilder.DropForeignKey(
                name: "FK_InvigilatorExaminationModule_Modules_ModuleId",
                table: "InvigilatorExaminationModule");

            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Faculties_FacultyId",
                table: "Modules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Modules",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Modules");

            migrationBuilder.RenameTable(
                name: "Modules",
                newName: "Module");

            migrationBuilder.RenameIndex(
                name: "IX_Modules_FacultyId",
                table: "Module",
                newName: "IX_Module_FacultyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Module",
                table: "Module",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ff7baf09-a790-41c2-8734-20e49cdb2fc3", "AQAAAAEAACcQAAAAEPEiMK0qo+W/4bUNpMpJY1SjofHYQP59uNkmrWe95H8mCUjVojjl0e+BMsDh0L7JJQ==" });

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateExaminationModule_Module_ModuleId",
                table: "CandidateExaminationModule",
                column: "ModuleId",
                principalTable: "Module",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationShift_Module_ModuleId",
                table: "ExaminationShift",
                column: "ModuleId",
                principalTable: "Module",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvigilatorExaminationModule_Module_ModuleId",
                table: "InvigilatorExaminationModule",
                column: "ModuleId",
                principalTable: "Module",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Module_Faculties_FacultyId",
                table: "Module",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "Id");
        }
    }
}
