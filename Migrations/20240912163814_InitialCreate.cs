using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIleHandling.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ImdbLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Action = table.Column<bool>(type: "bit", nullable: true),
                    Adventure = table.Column<bool>(type: "bit", nullable: true),
                    Comedy = table.Column<bool>(type: "bit", nullable: true),
                    Drama = table.Column<bool>(type: "bit", nullable: true),
                    Romance = table.Column<bool>(type: "bit", nullable: true),
                    Thriller = table.Column<bool>(type: "bit", nullable: true),
                    ScienceFiction = table.Column<bool>(type: "bit", nullable: true),
                    Animation = table.Column<bool>(type: "bit", nullable: true),
                    Fantasy = table.Column<bool>(type: "bit", nullable: true),
                    Horror = table.Column<bool>(type: "bit", nullable: true),
                    Musical = table.Column<bool>(type: "bit", nullable: true),
                    Mystery = table.Column<bool>(type: "bit", nullable: true),
                    Documentary = table.Column<bool>(type: "bit", nullable: true),
                    War = table.Column<bool>(type: "bit", nullable: true),
                    Crime = table.Column<bool>(type: "bit", nullable: true),
                    Western = table.Column<bool>(type: "bit", nullable: true),
                    FilmNoir = table.Column<bool>(type: "bit", nullable: true),
                    Childrens = table.Column<bool>(type: "bit", nullable: true),
                    Other = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.MovieId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Age = table.Column<int>(type: "int", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    Rating1 = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Ratings_Movies",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "MovieId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ratings_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_MovieId",
                table: "Ratings",
                column: "MovieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
