using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoadAppWEB.Migrations
{
    /// <inheritdoc />
    public partial class changeNodeRoadName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "mainline_id",
                table: "node");

            migrationBuilder.RenameColumn(
                name: "ramp_id",
                table: "node",
                newName: "road_id");

            migrationBuilder.AlterColumn<double>(
                name: "avg_length",
                table: "road",
                type: "float",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "road_id",
                table: "node",
                newName: "ramp_id");

            migrationBuilder.AlterColumn<float>(
                name: "avg_length",
                table: "road",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "mainline_id",
                table: "node",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
