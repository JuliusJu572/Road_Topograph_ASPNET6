using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoadAppWEB.Migrations
{
    /// <inheritdoc />
    public partial class addOverpass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "overpass",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    trunknetwork = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    maintenanceunit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    operatingunit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lanes = table.Column<int>(type: "int", nullable: false),
                    length = table.Column<double>(type: "float", nullable: true),
                    square = table.Column<double>(type: "float", nullable: true),
                    date = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_overpass", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "overpass");
        }
    }
}
