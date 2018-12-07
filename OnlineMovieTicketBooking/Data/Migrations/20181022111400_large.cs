using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OnlineMovieTicketBooking.Data.Migrations
{
    public partial class large : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Halls");

            migrationBuilder.AddColumn<bool>(
                name: "Large",
                table: "Halls",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Large",
                table: "Halls");

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Halls",
                nullable: false,
                defaultValue: 0);
        }
    }
}
