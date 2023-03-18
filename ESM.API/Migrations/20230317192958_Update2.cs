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
                name: "FK_InvigilatorShift_AspNetUsers_CreatedById",
                table: "InvigilatorShift");

            migrationBuilder.DropIndex(
                name: "IX_InvigilatorShift_CreatedById",
                table: "InvigilatorShift");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "InvigilatorShift");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "2fef428d-66fa-42ca-b7d7-e158646eb505", "AQAAAAEAACcQAAAAEFpEOAwCcKi7kEkLBAaHC1dAJeP790vyvEoIwd/87AQrYmuUnzVpbW4z4ZIUpuorKw==" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e18-c46f-4e76-8e77-69430f54d796"),
                column: "ConcurrencyStamp",
                value: "8f163522-f4f5-44fb-9264-2e214791710a");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e1a-7953-4790-8ebe-272e34a8fe18"),
                column: "ConcurrencyStamp",
                value: "12269493-09c1-429f-b602-a5a4ba2ad9b3");

            migrationBuilder.CreateIndex(
                name: "IX_InvigilatorShift_InvigilatorId",
                table: "InvigilatorShift",
                column: "InvigilatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvigilatorShift_AspNetUsers_InvigilatorId",
                table: "InvigilatorShift",
                column: "InvigilatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvigilatorShift_AspNetUsers_InvigilatorId",
                table: "InvigilatorShift");

            migrationBuilder.DropIndex(
                name: "IX_InvigilatorShift_InvigilatorId",
                table: "InvigilatorShift");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "InvigilatorShift",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "118850d9-949c-4641-bd87-c9fea7e35d5a", "AQAAAAEAACcQAAAAEBt0PQ08llP854eLNVnCQOMNew0hh5z4F26MotXurAC8sI6rZCfdSDgvSI0jdEje7g==" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e18-c46f-4e76-8e77-69430f54d796"),
                column: "ConcurrencyStamp",
                value: "5722d18b-9edd-41d2-8f6c-f3de101cfcf2");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e1a-7953-4790-8ebe-272e34a8fe18"),
                column: "ConcurrencyStamp",
                value: "011beeab-779f-4fc8-89e8-8b996be53a80");

            migrationBuilder.CreateIndex(
                name: "IX_InvigilatorShift_CreatedById",
                table: "InvigilatorShift",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_InvigilatorShift_AspNetUsers_CreatedById",
                table: "InvigilatorShift",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
