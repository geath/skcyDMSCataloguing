using Microsoft.EntityFrameworkCore.Migrations;

namespace skcyDMSCataloguing.Migrations
{
    public partial class initialload2custrelateddata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustOldAccountNo",
                table: "CustomerAccount");

            migrationBuilder.AddColumn<string>(
                name: "CustNewCIFNo",
                table: "CustomerAccount",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustOldCIFNo",
                table: "CustomerAccount",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustNewCIFNo",
                table: "CustomerAccount");

            migrationBuilder.DropColumn(
                name: "CustOldCIFNo",
                table: "CustomerAccount");

            migrationBuilder.AddColumn<string>(
                name: "CustOldAccountNo",
                table: "CustomerAccount",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
