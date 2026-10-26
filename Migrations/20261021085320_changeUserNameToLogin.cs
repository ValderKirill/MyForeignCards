using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyForeignCards.Migrations
{
    /// <inheritdoc />
    public partial class changeUserNameToLogin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "users",
                newName: "login");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "login",
                table: "users",
                newName: "name");
        }
    }
}
