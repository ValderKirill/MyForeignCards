using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyForeignCards.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                table: "words",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                table: "categories",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_words_user_id",
                table: "words",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_categories_user_id",
                table: "categories",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_categories_user_user_id",
                table: "categories",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_words_user_user_id",
                table: "words",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_categories_user_user_id",
                table: "categories");

            migrationBuilder.DropForeignKey(
                name: "fk_words_user_user_id",
                table: "words");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropIndex(
                name: "ix_words_user_id",
                table: "words");

            migrationBuilder.DropIndex(
                name: "ix_categories_user_id",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "words");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "categories");
        }
    }
}
