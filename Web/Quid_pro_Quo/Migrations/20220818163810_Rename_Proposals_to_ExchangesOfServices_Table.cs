using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quid_pro_Quo.Migrations
{
    public partial class Rename_Proposals_to_ExchangesOfServices_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Proposals");

            //migrationBuilder.DropTable(
            //    name: "ScalarReturn<int>");

            migrationBuilder.CreateTable(
                name: "ExchangesOfServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RequestingPostId = table.Column<int>(type: "INTEGER", nullable: false),
                    RequestedPostId = table.Column<int>(type: "INTEGER", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: true),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangesOfServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExchangesOfServices_Users_RequestedPostId",
                        column: x => x.RequestedPostId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExchangesOfServices_Users_RequestingPostId",
                        column: x => x.RequestingPostId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExchangesOfServices_RequestedPostId",
                table: "ExchangesOfServices",
                column: "RequestedPostId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangesOfServices_RequestingPostId",
                table: "ExchangesOfServices",
                column: "RequestingPostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExchangesOfServices");

            migrationBuilder.CreateTable(
                name: "Proposals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RequestedPostId = table.Column<int>(type: "INTEGER", nullable: false),
                    RequestingPostId = table.Column<int>(type: "INTEGER", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: true),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proposals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Proposals_Users_RequestedPostId",
                        column: x => x.RequestedPostId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Proposals_Users_RequestingPostId",
                        column: x => x.RequestingPostId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.CreateTable(
            //    name: "ScalarReturn<int>",
            //    columns: table => new
            //    {
            //        Value = table.Column<int>(type: "INTEGER", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_Proposals_RequestedPostId",
                table: "Proposals",
                column: "RequestedPostId");

            migrationBuilder.CreateIndex(
                name: "IX_Proposals_RequestingPostId",
                table: "Proposals",
                column: "RequestingPostId");
        }
    }
}
