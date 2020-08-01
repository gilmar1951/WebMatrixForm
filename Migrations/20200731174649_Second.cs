using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MatrixForm.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "Matrix");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Matrix",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Matrix",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Matrix");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Matrix");

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "Matrix",
                nullable: false,
                defaultValue: 0);
        }
    }
}
