using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Taste.DataAccess.Migrations
{
    public partial class addshoppingcardtoDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                          name: "shoppingCards");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shoppingCards");
        }
    }
}
