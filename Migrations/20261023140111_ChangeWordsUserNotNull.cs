using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyForeignCards.Migrations
{
    /// <inheritdoc />
    public partial class ChangeWordsUserNotNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_words_users_user_id",
                table: "words");

            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                table: "words",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_words_users_user_id",
                table: "words",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_words_users_user_id",
                table: "words");

            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                table: "words",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "fk_words_users_user_id",
                table: "words",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}
