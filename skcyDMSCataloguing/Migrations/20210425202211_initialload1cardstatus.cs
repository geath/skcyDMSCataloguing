using Microsoft.EntityFrameworkCore.Migrations;

namespace skcyDMSCataloguing.Migrations
{
    public partial class initialload1cardstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CardActive",
                table: "Customer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardActive",
                table: "Customer");
        }
    }
}
