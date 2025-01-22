using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class NewUserStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UC_Users",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "UC_Users",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UC_Users",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "UC_Users",
                table: "Users",
                column: "UserName",
                unique: true);
        }
    }
}
