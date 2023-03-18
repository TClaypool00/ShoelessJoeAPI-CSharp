using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoelessJoeAPI.DataAccess.Migrations
{
    public partial class AddedShoeImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShoeImage",
                columns: table => new
                {
                    ShoeImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RightShoeImage1 = table.Column<byte[]>(type: "longblob", nullable: true),
                    RightShoeImage2 = table.Column<byte[]>(type: "longblob", nullable: true),
                    LeftShoeImage1 = table.Column<byte[]>(type: "longblob", nullable: true),
                    LeftShoeImage2 = table.Column<byte[]>(type: "longblob", nullable: true),
                    ShoeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoeImage", x => x.ShoeImageId);
                    table.ForeignKey(
                        name: "FK_ShoeImage_Shoes_ShoeId",
                        column: x => x.ShoeId,
                        principalTable: "Shoes",
                        principalColumn: "ShoeId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ShoeImage_ShoeId",
                table: "ShoeImage",
                column: "ShoeId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoeImage");
        }
    }
}
