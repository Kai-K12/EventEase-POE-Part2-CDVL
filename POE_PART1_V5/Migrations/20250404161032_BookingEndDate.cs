using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POE_PART1_V5.Migrations
{
    /// <inheritdoc />
    public partial class BookingEndDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BookingEndDate",
                table: "Booking",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingEndDate",
                table: "Booking");
        }
    }
}
