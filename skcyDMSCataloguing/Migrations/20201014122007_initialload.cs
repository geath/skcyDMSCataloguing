using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace skcyDMSCataloguing.Migrations
{
    public partial class initialload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankAccount",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustAccountNo = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccount", x => x.ID);
                    table.UniqueConstraint("AK_BankAccount_CustAccountNo", x => x.CustAccountNo);
                });

            migrationBuilder.CreateTable(
                name: "BoxCreator",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatorSynthesisID = table.Column<int>(nullable: false),
                    CreatorName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxCreator", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CIFNo = table.Column<string>(nullable: false),
                    CustomerName = table.Column<string>(nullable: true),
                    CustomerNo = table.Column<string>(nullable: true),
                    CustomerIDN = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.ID);
                    table.UniqueConstraint("AK_Customer_CIFNo", x => x.CIFNo);
                });

            migrationBuilder.CreateTable(
                name: "Box",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoxDescription = table.Column<string>(nullable: true),
                    DateBoxCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    BoxCreatedBy = table.Column<string>(nullable: true),
                    BoxCreatorID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Box", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Box_BoxCreator_BoxCreatorID",
                        column: x => x.BoxCreatorID,
                        principalTable: "BoxCreator",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerAccount",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustOldAccountNo = table.Column<string>(nullable: true),
                    CustAccountType = table.Column<string>(nullable: true),
                    CustAccountStatus = table.Column<string>(nullable: true),
                    CustAccountRelationCode = table.Column<string>(nullable: true),
                    CustCIFNo = table.Column<string>(nullable: true),
                    CustAccountNo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerAccount", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CustomerAccount_BankAccount_CustAccountNo",
                        column: x => x.CustAccountNo,
                        principalTable: "BankAccount",
                        principalColumn: "CustAccountNo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerAccount_Customer_CustCIFNo",
                        column: x => x.CustCIFNo,
                        principalTable: "Customer",
                        principalColumn: "CIFNo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PrjHelix1",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Helix1Pool = table.Column<string>(nullable: true),
                    CustDataCIFNo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrjHelix1", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PrjHelix1_Customer_CustDataCIFNo",
                        column: x => x.CustDataCIFNo,
                        principalTable: "Customer",
                        principalColumn: "CIFNo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PrjVelocity1",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustDataCIFNo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrjVelocity1", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PrjVelocity1_Customer_CustDataCIFNo",
                        column: x => x.CustDataCIFNo,
                        principalTable: "Customer",
                        principalColumn: "CIFNo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PrjVelocity2",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustDataCIFNo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrjVelocity2", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PrjVelocity2_Customer_CustDataCIFNo",
                        column: x => x.CustDataCIFNo,
                        principalTable: "Customer",
                        principalColumn: "CIFNo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Folder",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FolderName = table.Column<string>(nullable: true),
                    FolderDescription = table.Column<string>(nullable: true),
                    BoxID = table.Column<int>(nullable: false),
                    CustDataCIFNo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folder", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Folder_Box_BoxID",
                        column: x => x.BoxID,
                        principalTable: "Box",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Folder_Customer_CustDataCIFNo",
                        column: x => x.CustDataCIFNo,
                        principalTable: "Customer",
                        principalColumn: "CIFNo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Box_BoxCreatorID",
                table: "Box",
                column: "BoxCreatorID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAccount_CustAccountNo",
                table: "CustomerAccount",
                column: "CustAccountNo");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAccount_CustCIFNo",
                table: "CustomerAccount",
                column: "CustCIFNo");

            migrationBuilder.CreateIndex(
                name: "IX_Folder_BoxID",
                table: "Folder",
                column: "BoxID");

            migrationBuilder.CreateIndex(
                name: "IX_Folder_CustDataCIFNo",
                table: "Folder",
                column: "CustDataCIFNo");

            migrationBuilder.CreateIndex(
                name: "IX_PrjHelix1_CustDataCIFNo",
                table: "PrjHelix1",
                column: "CustDataCIFNo");

            migrationBuilder.CreateIndex(
                name: "IX_PrjVelocity1_CustDataCIFNo",
                table: "PrjVelocity1",
                column: "CustDataCIFNo");

            migrationBuilder.CreateIndex(
                name: "IX_PrjVelocity2_CustDataCIFNo",
                table: "PrjVelocity2",
                column: "CustDataCIFNo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerAccount");

            migrationBuilder.DropTable(
                name: "Folder");

            migrationBuilder.DropTable(
                name: "PrjHelix1");

            migrationBuilder.DropTable(
                name: "PrjVelocity1");

            migrationBuilder.DropTable(
                name: "PrjVelocity2");

            migrationBuilder.DropTable(
                name: "BankAccount");

            migrationBuilder.DropTable(
                name: "Box");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "BoxCreator");
        }
    }
}
