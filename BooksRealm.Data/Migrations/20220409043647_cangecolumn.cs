using Microsoft.EntityFrameworkCore.Migrations;

namespace BooksRealm.Data.Migrations
{
    public partial class cangecolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Reviews",
                newName: "Content");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Reviews",
                newName: "Text");
        }
    }
}
