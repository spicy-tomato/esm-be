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
                name: "FK_InvigilatorShift_AspNetUsers_CreatedById",
                table: "InvigilatorShift");

            migrationBuilder.AlterColumn<string>(
                name: "InvigilatorId",
                table: "InvigilatorShift",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedById",
                table: "InvigilatorShift",
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
                values: new object[] { "b7edc780-6c81-413b-b7b8-41ae5c672139", "AQAAAAEAACcQAAAAED+Xu0s+98g0Pt6CJPMIWI2TNreUofbPC5Aso/SubHX99Vq8naCuubF6YhPD3L85tw==" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e18-c46f-4e76-8e77-69430f54d796"),
                column: "ConcurrencyStamp",
                value: "872fa1c8-2d53-4cb5-aebe-80e5a7306487");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e1a-7953-4790-8ebe-272e34a8fe18"),
                column: "ConcurrencyStamp",
                value: "bf9d551b-96a9-41bd-ad7a-f9d502d83e7a");

            migrationBuilder.AddForeignKey(
                name: "FK_InvigilatorShift_AspNetUsers_CreatedById",
                table: "InvigilatorShift",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvigilatorShift_AspNetUsers_CreatedById",
                table: "InvigilatorShift");

            migrationBuilder.UpdateData(
                table: "InvigilatorShift",
                keyColumn: "InvigilatorId",
                keyValue: null,
                column: "InvigilatorId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "InvigilatorId",
                table: "InvigilatorShift",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedById",
                table: "InvigilatorShift",
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
                values: new object[] { "3182f890-ebfa-47ec-9085-bd054d045641", "AQAAAAEAACcQAAAAEJ4KKJmacMsaKfxsbTi2kjkpr1yhGgwjpKvMYZX4sU2KmLkzhMv/Tp5n/hynbxJqEw==" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e18-c46f-4e76-8e77-69430f54d796"),
                column: "ConcurrencyStamp",
                value: "d01d1356-8ef7-43eb-a3ca-15e03853a8af");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e1a-7953-4790-8ebe-272e34a8fe18"),
                column: "ConcurrencyStamp",
                value: "689e0905-f6b2-45a1-b34b-b4397d663095");

            migrationBuilder.AddForeignKey(
                name: "FK_InvigilatorShift_AspNetUsers_CreatedById",
                table: "InvigilatorShift",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
