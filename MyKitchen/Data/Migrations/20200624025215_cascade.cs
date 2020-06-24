using Microsoft.EntityFrameworkCore.Migrations;

namespace MyKitchen.Data.Migrations
{
    public partial class cascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_FoodGroups_FoodGroupID",
                table: "FoodItems");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_FoodGroups_FoodGroupID",
                table: "FoodItems",
                column: "FoodGroupID",
                principalTable: "FoodGroups",
                principalColumn: "FoodGroupID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_FoodGroups_FoodGroupID",
                table: "FoodItems");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_FoodGroups_FoodGroupID",
                table: "FoodItems",
                column: "FoodGroupID",
                principalTable: "FoodGroups",
                principalColumn: "FoodGroupID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
