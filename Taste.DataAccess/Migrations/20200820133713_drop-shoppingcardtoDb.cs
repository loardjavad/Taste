using Microsoft.EntityFrameworkCore.Migrations;

namespace Taste.DataAccess.Migrations
{
    public partial class dropshoppingcardtoDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_shoppingCards",
                table: "shoppingCards");

            migrationBuilder.RenameTable(
                name: "shoppingCards",
                newName: "ShoppingCards");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCards",
                table: "ShoppingCards",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCards",
                table: "ShoppingCards");

            migrationBuilder.RenameTable(
                name: "ShoppingCards",
                newName: "shoppingCards");

            migrationBuilder.AddPrimaryKey(
                name: "PK_shoppingCards",
                table: "shoppingCards",
                column: "Id");
        }
    }
}
