using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceWeather.Bootstrap.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.AlterDatabase()
            .Annotation("MySql:CharSet", "utf8mb4");

        _ = migrationBuilder.CreateTable(
            name: "MagneticIndexReadings",
            columns: table => new
            {
                StartTimeUtc = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                Type = table.Column<string>(type: "char(1)", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Station = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                EndTimeUtc = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                Value = table.Column<double>(type: "double", precision: 3, scale: 2, nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_MagneticIndexReadings", x => new { x.StartTimeUtc, x.Type, x.Station });
            })
            .Annotation("MySql:CharSet", "utf8mb4");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropTable(
            name: "MagneticIndexReadings");
    }
}
