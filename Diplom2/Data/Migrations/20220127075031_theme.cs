using Microsoft.EntityFrameworkCore.Migrations;

namespace Diplom2.Data.Migrations
{
    public partial class theme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ThemeSite",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThemeSite",
                table: "AspNetUsers");
        }
    }
}
