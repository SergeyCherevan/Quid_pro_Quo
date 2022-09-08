using Microsoft.EntityFrameworkCore.Migrations;

namespace Quid_pro_Quo.Migrations
{
    public partial class Add_DateNumberOfReqPost_Columns_Edit_ReqPost_Relationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExchangesOfServices_Users_RequestedPostId",
                table: "ExchangesOfServices");

            migrationBuilder.DropForeignKey(
                name: "FK_ExchangesOfServices_Users_RequestingPostId",
                table: "ExchangesOfServices");

            //migrationBuilder.DropTable(
            //    name: "ScalarReturn<int>");

            migrationBuilder.AddColumn<int>(
                name: "DateNumberOfRequestedPost",
                table: "ExchangesOfServices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DateNumberOfRequestingPost",
                table: "ExchangesOfServices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangesOfServices_Posts_RequestedPostId",
                table: "ExchangesOfServices",
                column: "RequestedPostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangesOfServices_Posts_RequestingPostId",
                table: "ExchangesOfServices",
                column: "RequestingPostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExchangesOfServices_Posts_RequestedPostId",
                table: "ExchangesOfServices");

            migrationBuilder.DropForeignKey(
                name: "FK_ExchangesOfServices_Posts_RequestingPostId",
                table: "ExchangesOfServices");

            migrationBuilder.DropColumn(
                name: "DateNumberOfRequestedPost",
                table: "ExchangesOfServices");

            migrationBuilder.DropColumn(
                name: "DateNumberOfRequestingPost",
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

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangesOfServices_Users_RequestedPostId",
                table: "ExchangesOfServices",
                column: "RequestedPostId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangesOfServices_Users_RequestingPostId",
                table: "ExchangesOfServices",
                column: "RequestingPostId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
