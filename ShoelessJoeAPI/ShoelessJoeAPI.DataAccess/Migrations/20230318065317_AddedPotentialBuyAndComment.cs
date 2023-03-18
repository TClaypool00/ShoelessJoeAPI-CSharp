using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoelessJoeAPI.DataAccess.Migrations
{
    public partial class AddedPotentialBuyAndComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSold",
                table: "Shoes");

            migrationBuilder.CreateTable(
                name: "PotentialBuy",
                columns: table => new
                {
                    PotentialBuyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ShoeId = table.Column<int>(type: "int", nullable: false),
                    PotentialBuyerUserId = table.Column<int>(type: "int", nullable: false),
                    IsSold = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    DateSold = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PotentialBuy", x => x.PotentialBuyId);
                    table.ForeignKey(
                        name: "FK_PotentialBuy_Shoes_ShoeId",
                        column: x => x.ShoeId,
                        principalTable: "Shoes",
                        principalColumn: "ShoeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PotentialBuy_Users_PotentialBuyerUserId",
                        column: x => x.PotentialBuyerUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CommentText = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DatePosted = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PotentialBuyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comment_PotentialBuy_PotentialBuyId",
                        column: x => x.PotentialBuyId,
                        principalTable: "PotentialBuy",
                        principalColumn: "PotentialBuyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comment_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_PotentialBuyId",
                table: "Comment",
                column: "PotentialBuyId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_UserId",
                table: "Comment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PotentialBuy_PotentialBuyerUserId",
                table: "PotentialBuy",
                column: "PotentialBuyerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PotentialBuy_ShoeId",
                table: "PotentialBuy",
                column: "ShoeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "PotentialBuy");

            migrationBuilder.AddColumn<bool>(
                name: "IsSold",
                table: "Shoes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
