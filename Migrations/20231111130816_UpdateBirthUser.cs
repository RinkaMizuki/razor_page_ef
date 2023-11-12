using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CS51_ASP.NET_Razor_EF_1.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBirthUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateBirth",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateBirth",
                table: "Users");
        }
    }
}
