using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class ProfileImageRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_ProfileImage",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "ProfileImageId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ProfileImage",
                table: "Users",
                column: "ProfileImageId",
                principalTable: "ProfileImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_ProfileImage",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "ProfileImageId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ProfileImage",
                table: "Users",
                column: "ProfileImageId",
                principalTable: "ProfileImages",
                principalColumn: "Id");
        }
    }
}
