using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBankIdentityService.Migrations
{
    public partial class UserPrincipalName_Unique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "RoleModulesToPermissions");

            migrationBuilder.AlterColumn<string>(
                name: "UserPrincipal",
                table: "Users",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_UserPrincipal",
                table: "Users",
                column: "UserPrincipal");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Users_UserPrincipal",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "UserPrincipal",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "RoleModulesToPermissions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
