using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESM.API.Migrations
{
    public partial class Shift_RemoveStartAt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShiftGroups_Modules_ModuleId",
                table: "ShiftGroups");

            migrationBuilder.DropColumn(
                name: "StartAt",
                table: "Shifts");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModuleId",
                table: "ShiftGroups",
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

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftGroups_Modules_ModuleId",
                table: "ShiftGroups",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShiftGroups_Modules_ModuleId",
                table: "ShiftGroups");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartAt",
                table: "Shifts",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "ModuleId",
                table: "ShiftGroups",
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
                values: new object[] { "614cdc8d-5a8e-4849-a349-2659c071ee82", "AQAAAAEAACcQAAAAEJ1UV1St1OQ0Wr1lEvdwbEUJ18BRV4EQeP74RCBcWHV9OLDVlj3tAbnIr3CQAK8sdw==" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e18-c46f-4e76-8e77-69430f54d796"),
                column: "ConcurrencyStamp",
                value: "11f64a2e-0ee5-427c-941d-5051320dbc2b");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e1a-7953-4790-8ebe-272e34a8fe18"),
                column: "ConcurrencyStamp",
                value: "d7bd4ac6-c66d-4d33-b580-78d3894dc4c2");

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftGroups_Modules_ModuleId",
                table: "ShiftGroups",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id");
        }
    }
}
