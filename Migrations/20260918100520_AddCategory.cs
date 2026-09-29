using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyForeignCards.Migrations
{
    /// <inheritdoc />
    public partial class AddCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "words",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "words",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.AddColumn<Guid>(
                name: "category_id",
                table: "words",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_words_category_id",
                table: "words",
                column: "category_id");

            migrationBuilder.AddForeignKey(
                name: "fk_words_categories_category_id",
                table: "words",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_words_categories_category_id",
                table: "words");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropIndex(
                name: "ix_words_category_id",
                table: "words");

            migrationBuilder.DropColumn(
                name: "category_id",
                table: "words");

            migrationBuilder.InsertData(
                table: "words",
                columns: new[] { "id", "text", "translation" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Apple", "Яблоко" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Table", "Стол" }
                });
        }
    }
}
