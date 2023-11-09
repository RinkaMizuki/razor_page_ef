using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CS51_ASP.NET_Razor_EF_1.Migrations
{
    /// <inheritdoc />
    public partial class SeedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            for (int i = 1; i <= 150; i++)
            {
                migrationBuilder.InsertData(
                "Users",
                columns: new [] { "Id", "UserName", "Email", "EmailConfirmed", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnabled", "AccessFailedCount" },
                values: new object[] { Guid.NewGuid().ToString(), "User-" + i.ToString("D3"), "User-" + i.ToString("D3") + "@example.com", true, false, false, true, 0}
            );
            }
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
