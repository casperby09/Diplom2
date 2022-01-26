using Microsoft.EntityFrameworkCore.Migrations;

namespace Diplom2.Data.Migrations
{
    public partial class likeadd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dates_Items_ItemId",
                table: "Dates");

            migrationBuilder.DropForeignKey(
                name: "FK_Lines_Items_ItemId",
                table: "Lines");

            migrationBuilder.DropForeignKey(
                name: "FK_Logicals_Items_ItemId",
                table: "Logicals");

            migrationBuilder.DropForeignKey(
                name: "FK_Numbers_Items_ItemId",
                table: "Numbers");

            migrationBuilder.DropForeignKey(
                name: "FK_Texts_Items_ItemId",
                table: "Texts");

            migrationBuilder.DropIndex(
                name: "IX_Texts_ItemId",
                table: "Texts");

            migrationBuilder.DropIndex(
                name: "IX_Numbers_ItemId",
                table: "Numbers");

            migrationBuilder.DropIndex(
                name: "IX_Logicals_ItemId",
                table: "Logicals");

            migrationBuilder.DropIndex(
                name: "IX_Lines_ItemId",
                table: "Lines");

            migrationBuilder.DropIndex(
                name: "IX_Dates_ItemId",
                table: "Dates");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Texts");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Numbers");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Logicals");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Lines");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Dates");

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(nullable: true),
                    userId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Likes_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Likes_ItemId",
                table: "Likes",
                column: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Texts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Numbers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Logicals",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Lines",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Dates",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Texts_ItemId",
                table: "Texts",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Numbers_ItemId",
                table: "Numbers",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Logicals_ItemId",
                table: "Logicals",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Lines_ItemId",
                table: "Lines",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Dates_ItemId",
                table: "Dates",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dates_Items_ItemId",
                table: "Dates",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lines_Items_ItemId",
                table: "Lines",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Logicals_Items_ItemId",
                table: "Logicals",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Numbers_Items_ItemId",
                table: "Numbers",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Texts_Items_ItemId",
                table: "Texts",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
