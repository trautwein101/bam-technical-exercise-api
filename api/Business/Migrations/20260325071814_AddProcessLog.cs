using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StargateAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddProcessLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProcessLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Action = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessLog", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AstronautDetail",
                keyColumn: "Id",
                keyValue: 1,
                column: "CareerStartDate",
                value: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 1,
                column: "DutyStartDate",
                value: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Person_Name",
                table: "Person",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessLog");

            migrationBuilder.DropIndex(
                name: "IX_Person_Name",
                table: "Person");

            migrationBuilder.UpdateData(
                table: "AstronautDetail",
                keyColumn: "Id",
                keyValue: 1,
                column: "CareerStartDate",
                value: new DateTime(2026, 3, 24, 20, 42, 17, 549, DateTimeKind.Local).AddTicks(3564));

            migrationBuilder.UpdateData(
                table: "AstronautDuty",
                keyColumn: "Id",
                keyValue: 1,
                column: "DutyStartDate",
                value: new DateTime(2026, 3, 24, 20, 42, 17, 549, DateTimeKind.Local).AddTicks(3640));
        }
    }
}
