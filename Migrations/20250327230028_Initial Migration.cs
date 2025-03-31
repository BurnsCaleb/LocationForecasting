using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocationForecasting.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestLogs",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    RequestUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestTime = table.Column<DateTime>(type: "datetime", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestLogs", x => x.RequestId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestLogs");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
