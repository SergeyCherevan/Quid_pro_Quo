using Microsoft.EntityFrameworkCore.Migrations;

namespace Quid_pro_Quo.Migrations
{
    public partial class Set_IntNull_Type_of_OwnerId_Column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IoTs_Users_OwnerId",
                table: "IoTs");

            //migrationBuilder.DropTable(
            //    name: "ScalarReturn<int>");

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "IoTs",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_IoTs_Users_OwnerId",
                table: "IoTs",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IoTs_Users_OwnerId",
                table: "IoTs");

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "IoTs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            //migrationBuilder.CreateTable(
            //    name: "ScalarReturn<int>",
            //    columns: table => new
            //    {
            //        Value = table.Column<int>(type: "INTEGER", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //    });

            migrationBuilder.AddForeignKey(
                name: "FK_IoTs_Users_OwnerId",
                table: "IoTs",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
