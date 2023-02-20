using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESM.API.Migrations
{
    public partial class Module_AddDepartment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Faculties_FacultyId",
                table: "Modules");

            migrationBuilder.AlterColumn<Guid>(
                name: "FacultyId",
                table: "Modules",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "Modules",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7b7e41e1-4d63-4b28-acf5-8c1ba43cde23", "AQAAAAEAACcQAAAAEFoVREJzhoVjt551hAYwoHjU0THS/bD+QpRwwV9uSOLk+feH65qa5PX1AzGO+1LcPQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_Modules_DepartmentId",
                table: "Modules",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Departments_DepartmentId",
                table: "Modules",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Faculties_FacultyId",
                table: "Modules",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Departments_DepartmentId",
                table: "Modules");

            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Faculties_FacultyId",
                table: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_Modules_DepartmentId",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Modules");

            migrationBuilder.AlterColumn<Guid>(
                name: "FacultyId",
                table: "Modules",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c099dd61-b6ae-483d-9d8e-4b68520938ce", "AQAAAAEAACcQAAAAEKP08Xs4Qo15TI7EbRO1pllw9e/Daz2ijjJogeDoIbdVXd0zabGBd+Yn5X1wKJouOA==" });

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Faculties_FacultyId",
                table: "Modules",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "Id");
        }
    }
}
