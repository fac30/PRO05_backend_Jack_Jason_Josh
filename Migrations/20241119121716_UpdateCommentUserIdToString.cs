using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRO05_backend_Jack_Jason_Josh.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCommentUserIdToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_collection_user_UserId1",
                table: "collection");

            migrationBuilder.DropForeignKey(
                name: "FK_comment_user_UserId1",
                table: "comment");

            migrationBuilder.DropIndex(
                name: "IX_comment_UserId1",
                table: "comment");

            migrationBuilder.DropIndex(
                name: "IX_collection_UserId1",
                table: "collection");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "comment");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "collection");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "comment",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "collection",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_comment_UserId",
                table: "comment",
                column: "UserId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_comment_user_UserId",
                table: "comment",
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

            migrationBuilder.DropForeignKey(
                name: "FK_comment_user_UserId",
                table: "comment");

            migrationBuilder.DropIndex(
                name: "IX_comment_UserId",
                table: "comment");

            migrationBuilder.DropIndex(
                name: "IX_collection_UserId",
                table: "collection");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "comment",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "comment",
                type: "text",
                nullable: true);

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
                name: "IX_comment_UserId1",
                table: "comment",
                column: "UserId1");

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

            migrationBuilder.AddForeignKey(
                name: "FK_comment_user_UserId1",
                table: "comment",
                column: "UserId1",
                principalTable: "user",
                principalColumn: "Id");
        }
    }
}
