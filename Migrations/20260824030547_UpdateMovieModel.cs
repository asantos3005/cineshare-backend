using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cineshare_backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMovieModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "Movies");

            migrationBuilder.AddColumn<int>(
                name: "ExternalMovieId",
                table: "Reviews",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "ExternalMovieId",
                table: "Movies",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "ReleaseYear",
                table: "Movies",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalMovieId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ReleaseYear",
                table: "Movies");

            migrationBuilder.AlterColumn<int>(
                name: "ExternalMovieId",
                table: "Movies",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateOnly>(
                name: "ReleaseDate",
                table: "Movies",
                type: "date",
                nullable: true);
        }
    }
}
