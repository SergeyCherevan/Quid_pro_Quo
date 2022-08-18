using Microsoft.EntityFrameworkCore.Migrations;

namespace Quid_pro_Quo.Migrations
{
    public partial class Add_Status_Columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "ScalarReturn<int>");

            migrationBuilder.AddColumn<int>(
                name: "DoneStatus1",
                table: "ExchangesOfServices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DoneStatus2",
                table: "ExchangesOfServices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "NotViewed",
                table: "ExchangesOfServices",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ProposalStatus",
                table: "ExchangesOfServices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoneStatus1",
                table: "ExchangesOfServices");

            migrationBuilder.DropColumn(
                name: "DoneStatus2",
                table: "ExchangesOfServices");

            migrationBuilder.DropColumn(
                name: "NotViewed",
                table: "ExchangesOfServices");

            migrationBuilder.DropColumn(
                name: "ProposalStatus",
                table: "ExchangesOfServices");

            //migrationBuilder.CreateTable(
            //    name: "ScalarReturn<int>",
            //    columns: table => new
            //    {
            //        Value = table.Column<int>(type: "INTEGER", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //    });
        }
    }
}
