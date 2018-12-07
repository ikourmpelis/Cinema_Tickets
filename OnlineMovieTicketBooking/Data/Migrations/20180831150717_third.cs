using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OnlineMovieTicketBooking.Data.Migrations
{
    public partial class third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                table: "BookingTable",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MovieName",
                table: "BookingTable",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "BookingTable",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCancelled",
                table: "BookingTable");

            migrationBuilder.DropColumn(
                name: "MovieName",
                table: "BookingTable");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "BookingTable");
        }
    }
}
