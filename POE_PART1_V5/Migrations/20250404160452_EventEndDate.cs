using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POE_PART1_V5.Migrations
{
    /// <inheritdoc />
    public partial class EventEndDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EventEndDate",
                table: "Event",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventEndDate",
                table: "Event");
        }
    }
}
