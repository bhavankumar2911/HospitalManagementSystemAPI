using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagementSystemAPI.Migrations
{
    public partial class userandstaffmodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_Roles_RoleId",
                table: "Staffs");

            migrationBuilder.DropIndex(
                name: "IX_Staffs_RoleId",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Staffs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Staffs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_RoleId",
                table: "Staffs",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_Roles_RoleId",
                table: "Staffs",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
