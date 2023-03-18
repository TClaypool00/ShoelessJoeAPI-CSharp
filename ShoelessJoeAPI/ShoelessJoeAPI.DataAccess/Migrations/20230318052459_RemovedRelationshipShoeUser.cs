using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoelessJoeAPI.DataAccess.Migrations
{
    public partial class RemovedRelationshipShoeUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shoes_Users_SoldToUserUserId",
                table: "Shoes");

            migrationBuilder.DropIndex(
                name: "IX_Shoes_SoldToUserUserId",
                table: "Shoes");

            migrationBuilder.DropColumn(
                name: "SoldToUserUserId",
                table: "Shoes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SoldToUserUserId",
                table: "Shoes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shoes_SoldToUserUserId",
                table: "Shoes",
                column: "SoldToUserUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shoes_Users_SoldToUserUserId",
                table: "Shoes",
                column: "SoldToUserUserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
