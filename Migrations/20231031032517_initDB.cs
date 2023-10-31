using System;
using Bogus;
using Bogus.DataSets;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CS51_ASP.NET_Razor_EF_1.Migrations
{
    /// <inheritdoc />
    public partial class initDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Content = table.Column<string>(type: "ntext", nullable: true),
                    PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articles", x => x.Id);
                });


            Randomizer.Seed = new Random(8675309);
            for (int i = 0; i <= 150; i++)
            {
                var mockArticle = new Faker<Article>()
                .RuleFor(a => a.Title, f => f.Lorem.Sentence())
                .RuleFor(a => a.Content, f => f.Lorem.Sentence())
                .RuleFor(a => a.PublishedDate, f => f.Date.Between(DateTime.Now, new DateTime(2025, 12, 31)));
            var mA = mockArticle.Generate(); // tạo ra một đối tượng được fake data
            migrationBuilder.InsertData(
                "articles",
                new string[] { "Title", "Content", "PublishedDate" },
                new object[] { mA.Title, mA.Content, mA.PublishedDate }
            );
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "articles");
        }
    }
}
