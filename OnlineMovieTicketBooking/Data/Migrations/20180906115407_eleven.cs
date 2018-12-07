using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OnlineMovieTicketBooking.Data.Migrations
{
    public partial class eleven : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hall1",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MovieDate = table.Column<DateTime>(nullable: false),
                    MovieDetailsId = table.Column<int>(nullable: true),
                    MovieID = table.Column<int>(nullable: false),
                    MovieName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hall1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hall1_MovieDetails_MovieDetailsId",
                        column: x => x.MovieDetailsId,
                        principalTable: "MovieDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hall1_MovieDetailsId",
                table: "Hall1",
                column: "MovieDetailsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hall1");
        }
    }
}
