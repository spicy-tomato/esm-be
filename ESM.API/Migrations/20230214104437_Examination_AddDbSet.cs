using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESM.API.Migrations
{
    public partial class Examination_AddDbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CandidateExaminationModule_Examination_ExaminationId",
                table: "CandidateExaminationModule");

            migrationBuilder.DropForeignKey(
                name: "FK_Examination_AspNetUsers_CreatedById",
                table: "Examination");

            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationShift_Examination_ExaminationId",
                table: "ExaminationShift");

            migrationBuilder.DropForeignKey(
                name: "FK_InvigilatorExaminationModule_Examination_ExaminationId",
                table: "InvigilatorExaminationModule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Examination",
                table: "Examination");

            migrationBuilder.RenameTable(
                name: "Examination",
                newName: "Examinations");

            migrationBuilder.RenameIndex(
                name: "IX_Examination_CreatedById",
                table: "Examinations",
                newName: "IX_Examinations_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Examinations",
                table: "Examinations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateExaminationModule_Examinations_ExaminationId",
                table: "CandidateExaminationModule",
                column: "ExaminationId",
                principalTable: "Examinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Examinations_AspNetUsers_CreatedById",
                table: "Examinations",
                column: "CreatedById",
                principalTable: "AspNetUsers",
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
                name: "FK_InvigilatorExaminationModule_Examinations_ExaminationId",
                table: "InvigilatorExaminationModule",
                column: "ExaminationId",
                principalTable: "Examinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CandidateExaminationModule_Examinations_ExaminationId",
                table: "CandidateExaminationModule");

            migrationBuilder.DropForeignKey(
                name: "FK_Examinations_AspNetUsers_CreatedById",
                table: "Examinations");

            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationShift_Examinations_ExaminationId",
                table: "ExaminationShift");

            migrationBuilder.DropForeignKey(
                name: "FK_InvigilatorExaminationModule_Examinations_ExaminationId",
                table: "InvigilatorExaminationModule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Examinations",
                table: "Examinations");

            migrationBuilder.RenameTable(
                name: "Examinations",
                newName: "Examination");

            migrationBuilder.RenameIndex(
                name: "IX_Examinations_CreatedById",
                table: "Examination",
                newName: "IX_Examination_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Examination",
                table: "Examination",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateExaminationModule_Examination_ExaminationId",
                table: "CandidateExaminationModule",
                column: "ExaminationId",
                principalTable: "Examination",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Examination_AspNetUsers_CreatedById",
                table: "Examination",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationShift_Examination_ExaminationId",
                table: "ExaminationShift",
                column: "ExaminationId",
                principalTable: "Examination",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvigilatorExaminationModule_Examination_ExaminationId",
                table: "InvigilatorExaminationModule",
                column: "ExaminationId",
                principalTable: "Examination",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
