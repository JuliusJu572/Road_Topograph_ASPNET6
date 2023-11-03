using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoadAppWEB.Migrations
{
    /// <inheritdoc />
    public partial class addRamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "mainline",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "overpass_id",
                table: "mainline",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "code",
                table: "mainline");

            migrationBuilder.DropColumn(
                name: "overpass_id",
                table: "mainline");
        }
    }
}
