using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coursework_server.Data.Migrations
{
    public partial class RenameThemeToTopic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Collections_Themes_ThemeId",
                table: "Collections");

            migrationBuilder.DropTable(
                name: "Themes");

            migrationBuilder.RenameColumn(
                name: "ThemeId",
                table: "Collections",
                newName: "TopicId");

            migrationBuilder.RenameIndex(
                name: "IX_Collections_ThemeId",
                table: "Collections",
                newName: "IX_Collections_TopicId");

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Collections_Topics_TopicId",
                table: "Collections",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Collections_Topics_TopicId",
                table: "Collections");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.RenameColumn(
                name: "TopicId",
                table: "Collections",
                newName: "ThemeId");

            migrationBuilder.RenameIndex(
                name: "IX_Collections_TopicId",
                table: "Collections",
                newName: "IX_Collections_ThemeId");

            migrationBuilder.CreateTable(
                name: "Themes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Themes", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Collections_Themes_ThemeId",
                table: "Collections",
                column: "ThemeId",
                principalTable: "Themes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
