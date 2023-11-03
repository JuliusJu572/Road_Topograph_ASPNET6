using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoadAppWEB.Migrations
{
    /// <inheritdoc />
    public partial class resultDataTest3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllNodesRes",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    node_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    hub_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    span = table.Column<float>(type: "real", nullable: false),
                    span2hub = table.Column<float>(type: "real", nullable: false),
                    direction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllNodesRes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "HubNodeRes",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    starthub_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    endhub_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    span = table.Column<float>(type: "real", nullable: false),
                    velocity = table.Column<float>(type: "real", nullable: false),
                    direction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HubNodeRes", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllNodesRes");

            migrationBuilder.DropTable(
                name: "HubNodeRes");
        }
    }
}
