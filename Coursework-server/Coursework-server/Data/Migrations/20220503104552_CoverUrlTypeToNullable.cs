using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coursework_server.Data.Migrations
{
    public partial class CoverUrlTypeToNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CoverURL",
                table: "Collections",
                newName: "CoverUrl");

            migrationBuilder.AlterColumn<string>(
                name: "CoverUrl",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CoverUrl",
                table: "Collections",
                newName: "CoverURL");

            migrationBuilder.AlterColumn<string>(
                name: "CoverURL",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
