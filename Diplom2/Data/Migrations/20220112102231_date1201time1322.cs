using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Diplom2.Data.Migrations
{
    public partial class date1201time1322 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CollectionIsDeleted",
                table: "Items");

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Texts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Numbers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Logicals",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Lines",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Dates",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ValueDates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<DateTime>(nullable: false),
                    DateId = table.Column<int>(nullable: true),
                    ItemId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValueDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ValueDates_Dates_DateId",
                        column: x => x.DateId,
                        principalTable: "Dates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ValueDates_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ValueLines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(nullable: true),
                    LineId = table.Column<int>(nullable: true),
                    ItemId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValueLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ValueLines_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ValueLines_Lines_LineId",
                        column: x => x.LineId,
                        principalTable: "Lines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ValueLogicals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<bool>(nullable: false),
                    LogicalId = table.Column<int>(nullable: true),
                    ItemId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValueLogicals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ValueLogicals_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ValueLogicals_Logicals_LogicalId",
                        column: x => x.LogicalId,
                        principalTable: "Logicals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ValueNumbers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<int>(nullable: false),
                    NumberId = table.Column<int>(nullable: true),
                    ItemId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValueNumbers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ValueNumbers_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ValueNumbers_Numbers_NumberId",
                        column: x => x.NumberId,
                        principalTable: "Numbers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ValueTexts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(nullable: true),
                    TextId = table.Column<int>(nullable: true),
                    ItemId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValueTexts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ValueTexts_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ValueTexts_Texts_TextId",
                        column: x => x.TextId,
                        principalTable: "Texts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ValueDates_DateId",
                table: "ValueDates",
                column: "DateId");

            migrationBuilder.CreateIndex(
                name: "IX_ValueDates_ItemId",
                table: "ValueDates",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ValueLines_ItemId",
                table: "ValueLines",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ValueLines_LineId",
                table: "ValueLines",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_ValueLogicals_ItemId",
                table: "ValueLogicals",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ValueLogicals_LogicalId",
                table: "ValueLogicals",
                column: "LogicalId");

            migrationBuilder.CreateIndex(
                name: "IX_ValueNumbers_ItemId",
                table: "ValueNumbers",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ValueNumbers_NumberId",
                table: "ValueNumbers",
                column: "NumberId");

            migrationBuilder.CreateIndex(
                name: "IX_ValueTexts_ItemId",
                table: "ValueTexts",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ValueTexts_TextId",
                table: "ValueTexts",
                column: "TextId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "ValueDates");

            migrationBuilder.DropTable(
                name: "ValueLines");

            migrationBuilder.DropTable(
                name: "ValueLogicals");

            migrationBuilder.DropTable(
                name: "ValueNumbers");

            migrationBuilder.DropTable(
                name: "ValueTexts");

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

            migrationBuilder.AddColumn<bool>(
                name: "CollectionIsDeleted",
                table: "Items",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
