using Microsoft.EntityFrameworkCore.Migrations;

namespace Quid_pro_Quo.Migrations
{
    public partial class Add_LatLngZoom_Columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PerformServiceInPlaceLat",
                table: "Posts",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PerformServiceInPlaceLng",
                table: "Posts",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PerformServiceInPlaceZoom",
                table: "Posts",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PerformServiceInPlaceLat",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PerformServiceInPlaceLng",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PerformServiceInPlaceZoom",
                table: "Posts");
        }
    }
}
