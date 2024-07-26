using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagementSystemAPI.Migrations
{
    public partial class phonestaff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Staffs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Staffs");
        }
    }
}
