using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocationForecasting.Migrations
{
    /// <inheritdoc />
    public partial class AddExceptionLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RequestTime",
                table: "RequestLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ExceptionLogs",
                columns: table => new
                {
                    SettingsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExceptionMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StackTrace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExceptionTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionLogs", x => x.SettingsId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExceptionLogs");

            migrationBuilder.DropColumn(
                name: "RequestTime",
                table: "RequestLogs");
        }
    }
}
