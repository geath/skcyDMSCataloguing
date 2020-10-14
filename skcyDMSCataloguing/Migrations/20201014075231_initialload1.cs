using Microsoft.EntityFrameworkCore.Migrations;

namespace skcyDMSCataloguing.Migrations
{
    public partial class initialload1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_BankAccount_CustAccountNo",
                table: "BankAccount");

            migrationBuilder.AlterColumn<string>(
                name: "CustAccountNo",
                table: "BankAccount",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CustAccountNo",
                table: "BankAccount",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_BankAccount_CustAccountNo",
                table: "BankAccount",
                column: "CustAccountNo");
        }
    }
}
