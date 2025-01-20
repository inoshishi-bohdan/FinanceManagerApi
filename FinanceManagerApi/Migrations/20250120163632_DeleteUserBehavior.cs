using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class DeleteUserBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Users",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Incomes_Users",
                table: "Incomes");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Users",
                table: "Expenses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Incomes_Users",
                table: "Incomes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Users",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Incomes_Users",
                table: "Incomes");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Users",
                table: "Expenses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Incomes_Users",
                table: "Incomes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
