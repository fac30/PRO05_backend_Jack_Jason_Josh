using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRO05_backend_Jack_Jason_Josh.Migrations
{
    /// <inheritdoc />
    public partial class changeCapitalisationOfColourName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "colourName",
                table: "colour",
                newName: "ColourName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ColourName",
                table: "colour",
                newName: "colourName");
        }
    }
}
