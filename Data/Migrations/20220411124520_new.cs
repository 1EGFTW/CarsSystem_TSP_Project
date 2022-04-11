using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarsSystem_TSP_Project.Data.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Services_ServicesServiceId",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "ServicesServiceId",
                table: "Cars",
                newName: "ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_ServicesServiceId",
                table: "Cars",
                newName: "IX_Cars_ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Services_ServiceId",
                table: "Cars",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Services_ServiceId",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "Cars",
                newName: "ServicesServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_ServiceId",
                table: "Cars",
                newName: "IX_Cars_ServicesServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Services_ServicesServiceId",
                table: "Cars",
                column: "ServicesServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
