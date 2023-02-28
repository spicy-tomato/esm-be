using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESM.API.Migrations
{
    public partial class ExaminationShift_AddTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CandidateShift_ExaminationShift_ExaminationShiftId",
                table: "CandidateShift");

            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationShift_Examinations_ExaminationId",
                table: "ExaminationShift");

            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationShift_Modules_ModuleId",
                table: "ExaminationShift");

            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationShift_Rooms_RoomId",
                table: "ExaminationShift");

            migrationBuilder.DropForeignKey(
                name: "FK_InvigilatorShift_ExaminationShift_ExaminationShiftId",
                table: "InvigilatorShift");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExaminationShift",
                table: "ExaminationShift");

            migrationBuilder.RenameTable(
                name: "ExaminationShift",
                newName: "ExaminationShifts");

            migrationBuilder.RenameIndex(
                name: "IX_ExaminationShift_RoomId",
                table: "ExaminationShifts",
                newName: "IX_ExaminationShifts_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_ExaminationShift_ModuleId",
                table: "ExaminationShifts",
                newName: "IX_ExaminationShifts_ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_ExaminationShift_ExaminationId",
                table: "ExaminationShifts",
                newName: "IX_ExaminationShifts_ExaminationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExaminationShifts",
                table: "ExaminationShifts",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "74f3f91a-1aae-4bff-8b69-c0f7e27ac54c", "AQAAAAEAACcQAAAAEFJtuyHa3rB4gXItRkc4H4QKgIzTodmwlnKVFmvP8tV/fi2uoEU2O5ubPCRe1tYNAg==" });

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateShift_ExaminationShifts_ExaminationShiftId",
                table: "CandidateShift",
                column: "ExaminationShiftId",
                principalTable: "ExaminationShifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationShifts_Examinations_ExaminationId",
                table: "ExaminationShifts",
                column: "ExaminationId",
                principalTable: "Examinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationShifts_Modules_ModuleId",
                table: "ExaminationShifts",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationShifts_Rooms_RoomId",
                table: "ExaminationShifts",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvigilatorShift_ExaminationShifts_ExaminationShiftId",
                table: "InvigilatorShift",
                column: "ExaminationShiftId",
                principalTable: "ExaminationShifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CandidateShift_ExaminationShifts_ExaminationShiftId",
                table: "CandidateShift");

            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationShifts_Examinations_ExaminationId",
                table: "ExaminationShifts");

            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationShifts_Modules_ModuleId",
                table: "ExaminationShifts");

            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationShifts_Rooms_RoomId",
                table: "ExaminationShifts");

            migrationBuilder.DropForeignKey(
                name: "FK_InvigilatorShift_ExaminationShifts_ExaminationShiftId",
                table: "InvigilatorShift");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExaminationShifts",
                table: "ExaminationShifts");

            migrationBuilder.RenameTable(
                name: "ExaminationShifts",
                newName: "ExaminationShift");

            migrationBuilder.RenameIndex(
                name: "IX_ExaminationShifts_RoomId",
                table: "ExaminationShift",
                newName: "IX_ExaminationShift_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_ExaminationShifts_ModuleId",
                table: "ExaminationShift",
                newName: "IX_ExaminationShift_ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_ExaminationShifts_ExaminationId",
                table: "ExaminationShift",
                newName: "IX_ExaminationShift_ExaminationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExaminationShift",
                table: "ExaminationShift",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9d6c4a6c-4594-4682-a097-0a943ad1429d", "AQAAAAEAACcQAAAAEL5jEq6Ya0aSvFSIswTdpgVQaJ9UIRU3UIfydi/orP1ive1mf+Mu+IpZQsXnffsE9g==" });

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateShift_ExaminationShift_ExaminationShiftId",
                table: "CandidateShift",
                column: "ExaminationShiftId",
                principalTable: "ExaminationShift",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationShift_Examinations_ExaminationId",
                table: "ExaminationShift",
                column: "ExaminationId",
                principalTable: "Examinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationShift_Modules_ModuleId",
                table: "ExaminationShift",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationShift_Rooms_RoomId",
                table: "ExaminationShift",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvigilatorShift_ExaminationShift_ExaminationShiftId",
                table: "InvigilatorShift",
                column: "ExaminationShiftId",
                principalTable: "ExaminationShift",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
