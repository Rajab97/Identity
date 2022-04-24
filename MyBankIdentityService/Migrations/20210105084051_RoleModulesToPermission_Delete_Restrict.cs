using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBankIdentityService.Migrations
{
    public partial class RoleModulesToPermission_Delete_Restrict : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleModulesToPermissions_Roles_RoleId",
                table: "RoleModulesToPermissions");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleModulesToPermissions_Roles_RoleId",
                table: "RoleModulesToPermissions",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleModulesToPermissions_Roles_RoleId",
                table: "RoleModulesToPermissions");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleModulesToPermissions_Roles_RoleId",
                table: "RoleModulesToPermissions",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
