using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagementSystemAPI.Migrations
{
    public partial class noofdaystoprescriptionitem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IntervalInHours",
                table: "PrescriptionItems",
                newName: "NoOfDays");

            migrationBuilder.AddColumn<int>(
                name: "ConsumingInterval",
                table: "PrescriptionItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsumingInterval",
                table: "PrescriptionItems");

            migrationBuilder.RenameColumn(
                name: "NoOfDays",
                table: "PrescriptionItems",
                newName: "IntervalInHours");
        }
    }
}
