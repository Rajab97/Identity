using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBankIdentityService.Migrations
{
    public partial class User_UserPrincipalName_IsUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Users_UserPrincipal",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserPrincipal",
                table: "Users",
                column: "UserPrincipal",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_UserPrincipal",
                table: "Users");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_UserPrincipal",
                table: "Users",
                column: "UserPrincipal");
        }
    }
}
