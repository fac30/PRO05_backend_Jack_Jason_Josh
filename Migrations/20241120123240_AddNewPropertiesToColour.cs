using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRO05_backend_Jack_Jason_Josh.Migrations
{
    /// <inheritdoc />
    public partial class AddNewPropertiesToColour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "colourName",
                table: "colour",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "colourName",
                table: "colour");
        }
    }
}
