using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRO05_backend_Jack_Jason_Josh.Migrations
{
    /// <inheritdoc />
    public partial class updateUserIdToStringAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_collection_user_UserId1",
                table: "collection");

            migrationBuilder.DropIndex(
                name: "IX_collection_UserId1",
                table: "collection");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "collection");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "collection",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_collection_UserId",
                table: "collection",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_collection_user_UserId",
                table: "collection",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_collection_user_UserId",
                table: "collection");

            migrationBuilder.DropIndex(
                name: "IX_collection_UserId",
                table: "collection");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "collection",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "collection",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_collection_UserId1",
                table: "collection",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_collection_user_UserId1",
                table: "collection",
                column: "UserId1",
                principalTable: "user",
                principalColumn: "Id");
        }
    }
}
