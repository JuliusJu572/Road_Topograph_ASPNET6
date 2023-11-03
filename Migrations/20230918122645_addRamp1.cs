using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoadAppWEB.Migrations
{
    /// <inheritdoc />
    public partial class addRamp1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ramp",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    viaduct_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    overpass_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    direction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    start_node = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    end_node = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    linknode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    avg_length = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ramp", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ramp");
        }
    }
}
