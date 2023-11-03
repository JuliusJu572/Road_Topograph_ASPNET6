using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoadAppWEB.Migrations
{
    /// <inheritdoc />
    public partial class addFacility_Road : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "facility",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_facility", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "road",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    facility_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    direction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    start_node = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    start_linknode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    end_node = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    end_linknode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    avg_length = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_road", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "facility");

            migrationBuilder.DropTable(
                name: "road");
        }
    }
}
