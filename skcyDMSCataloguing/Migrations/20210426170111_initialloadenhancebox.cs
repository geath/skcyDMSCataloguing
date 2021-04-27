using Microsoft.EntityFrameworkCore.Migrations;

namespace skcyDMSCataloguing.Migrations
{
    public partial class initialloadenhancebox : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BoxTypeA",
                table: "Box",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "BoxTypeBoolA",
                table: "Box",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "BoxTypeNumberA",
                table: "Box",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BoxTypeTextA",
                table: "Box",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "Box",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoxTypeA",
                table: "Box");

            migrationBuilder.DropColumn(
                name: "BoxTypeBoolA",
                table: "Box");

            migrationBuilder.DropColumn(
                name: "BoxTypeNumberA",
                table: "Box");

            migrationBuilder.DropColumn(
                name: "BoxTypeTextA",
                table: "Box");

            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "Box");
        }
    }
}
