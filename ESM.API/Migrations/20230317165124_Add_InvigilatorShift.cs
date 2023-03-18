using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESM.API.Migrations
{
    public partial class Add_InvigilatorShift : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c1445c45-43c9-4cff-afea-8f944ad9d9dc", "AQAAAAEAACcQAAAAEN6+d0q7czuZ0MTQlu+6Rm72h+iogPKNM/Y4kwt29WESYPW7xjLF+b42xD2fBR5dYg==" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e18-c46f-4e76-8e77-69430f54d796"),
                column: "ConcurrencyStamp",
                value: "5690a794-dce0-4067-ae80-ccac243e7c6c");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("08db1e1a-7953-4790-8ebe-272e34a8fe18"),
                column: "ConcurrencyStamp",
                value: "2457828c-101c-4b5f-b96e-dcdc5b1ce056");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
