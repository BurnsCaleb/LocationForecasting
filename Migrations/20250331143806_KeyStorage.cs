using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocationForecasting.Migrations
{
    /// <inheritdoc />
    public partial class KeyStorage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KeyStorage",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyStorage", x => x.Key);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KeyStorage");
        }
    }
}
