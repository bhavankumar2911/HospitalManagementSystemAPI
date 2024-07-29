using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagementSystemAPI.Migrations
{
    public partial class appointmentstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppointmentStatus",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentStatus",
                table: "Appointments");
        }
    }
}
