using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProfileImageIdDefaultConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DECLARE @ConstraintName NVARCHAR(255)
                SELECT @ConstraintName = name
                FROM sys.default_constraints
                WHERE parent_object_id = OBJECT_ID('Users')
                  AND parent_column_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'ProfileImageId');

                IF @ConstraintName IS NOT NULL
                BEGIN
                    DECLARE @Sql NVARCHAR(MAX) = 'ALTER TABLE Users DROP CONSTRAINT ' + @ConstraintName
                    EXEC sp_executesql @Sql
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ProfileImageId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 1, 
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
