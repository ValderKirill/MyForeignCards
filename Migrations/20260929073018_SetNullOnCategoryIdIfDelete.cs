using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyForeignCards.Migrations
{
    /// <inheritdoc />
    public partial class SetNullOnCategoryIdIfDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_words_categories_category_id",
                table: "words");

            migrationBuilder.AddForeignKey(
                name: "fk_words_categories_category_id",
                table: "words",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_words_categories_category_id",
                table: "words");

            migrationBuilder.AddForeignKey(
                name: "fk_words_categories_category_id",
                table: "words",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "id");
        }
    }
}
