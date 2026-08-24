using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cineshare_backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMovieModelAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Movies_MovieId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ExternalMovieId",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "Reviews",
                newName: "InternalMovieId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_MovieId",
                table: "Reviews",
                newName: "IX_Reviews_InternalMovieId");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "Movies",
                newName: "InternalMovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Movies_InternalMovieId",
                table: "Reviews",
                column: "InternalMovieId",
                principalTable: "Movies",
                principalColumn: "InternalMovieId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Movies_InternalMovieId",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "InternalMovieId",
                table: "Reviews",
                newName: "MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_InternalMovieId",
                table: "Reviews",
                newName: "IX_Reviews_MovieId");

            migrationBuilder.RenameColumn(
                name: "InternalMovieId",
                table: "Movies",
                newName: "MovieId");

            migrationBuilder.AddColumn<int>(
                name: "ExternalMovieId",
                table: "Reviews",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Movies_MovieId",
                table: "Reviews",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
