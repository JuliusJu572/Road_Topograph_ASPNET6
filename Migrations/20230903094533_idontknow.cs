using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoadAppWEB.Migrations
{
    /// <inheritdoc />
    public partial class idontknow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "longtitude",
                table: "node",
                newName: "longitude");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "longitude",
                table: "node",
                newName: "longtitude");
        }
    }
}
