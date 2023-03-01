using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESM.API.Migrations
{
    public partial class ExaminationShiftGroup_AddDbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationShiftGroup_Examinations_ExaminationId",
                table: "ExaminationShiftGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationShiftGroup_Modules_ModuleId",
                table: "ExaminationShiftGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationShifts_ExaminationShiftGroup_ExaminationShiftGrou~",
                table: "ExaminationShifts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExaminationShiftGroup",
                table: "ExaminationShiftGroup");

            migrationBuilder.RenameTable(
                name: "ExaminationShiftGroup",
                newName: "ExaminationShiftGroups");

            migrationBuilder.RenameIndex(
                name: "IX_ExaminationShiftGroup_ModuleId",
                table: "ExaminationShiftGroups",
                newName: "IX_ExaminationShiftGroups_ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_ExaminationShiftGroup_ExaminationId",
                table: "ExaminationShiftGroups",
                newName: "IX_ExaminationShiftGroups_ExaminationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExaminationShiftGroups",
                table: "ExaminationShiftGroups",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "754176be-b464-4eff-a507-3f39dbdce688", "AQAAAAEAACcQAAAAEATyxwvRpENZNz+UjaecE+k8VxvpkqPtBrFCMz9VNcfE1YjHigPRg91sGKY39hCP/A==" });

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationShiftGroups_Examinations_ExaminationId",
                table: "ExaminationShiftGroups",
                column: "ExaminationId",
                principalTable: "Examinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationShiftGroups_Modules_ModuleId",
                table: "ExaminationShiftGroups",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationShifts_ExaminationShiftGroups_ExaminationShiftGro~",
                table: "ExaminationShifts",
                column: "ExaminationShiftGroupId",
                principalTable: "ExaminationShiftGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationShiftGroups_Examinations_ExaminationId",
                table: "ExaminationShiftGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationShiftGroups_Modules_ModuleId",
                table: "ExaminationShiftGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationShifts_ExaminationShiftGroups_ExaminationShiftGro~",
                table: "ExaminationShifts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExaminationShiftGroups",
                table: "ExaminationShiftGroups");

            migrationBuilder.RenameTable(
                name: "ExaminationShiftGroups",
                newName: "ExaminationShiftGroup");

            migrationBuilder.RenameIndex(
                name: "IX_ExaminationShiftGroups_ModuleId",
                table: "ExaminationShiftGroup",
                newName: "IX_ExaminationShiftGroup_ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_ExaminationShiftGroups_ExaminationId",
                table: "ExaminationShiftGroup",
                newName: "IX_ExaminationShiftGroup_ExaminationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExaminationShiftGroup",
                table: "ExaminationShiftGroup",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3d2dc4ec-e733-4992-8a06-089edd20b556", "AQAAAAEAACcQAAAAELT3rFeQGlxhqSEHLcvzc1N/uxQQcSj2TqouT5lfCXTolWKjt/0tCCA0Wt0JMQ57ZQ==" });

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationShiftGroup_Examinations_ExaminationId",
                table: "ExaminationShiftGroup",
                column: "ExaminationId",
                principalTable: "Examinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationShiftGroup_Modules_ModuleId",
                table: "ExaminationShiftGroup",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationShifts_ExaminationShiftGroup_ExaminationShiftGrou~",
                table: "ExaminationShifts",
                column: "ExaminationShiftGroupId",
                principalTable: "ExaminationShiftGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
