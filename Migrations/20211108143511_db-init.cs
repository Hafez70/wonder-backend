using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WTLicVerify.Migrations
{
    public partial class dbinit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EnvatoAccess",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    activationCode = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    refresh_token = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    token_type = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    access_token = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    machineId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    extenstion_version = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    extenstion_name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    application_version = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    application_name = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    machine_name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    expires_in = table.Column<int>(type: "int", nullable: false),
                    activated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    connected_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    activate = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnvatoAccess", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SaleItem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    number_of_sales = table.Column<int>(type: "int", nullable: false),
                    author_username = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    author_url = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    url = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    updated_at = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    site = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleItem", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AuthorSale",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    amount = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sold_at = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    license = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    support_amount = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    supported_until = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ItemId = table.Column<long>(type: "bigint", nullable: true),
                    EnvatoAccessId = table.Column<long>(type: "bigint", nullable: true),
                    code = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    purchase_count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorSale", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorSale_EnvatoAccess_EnvatoAccessId",
                        column: x => x.EnvatoAccessId,
                        principalTable: "EnvatoAccess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuthorSale_SaleItem_ItemId",
                        column: x => x.ItemId,
                        principalTable: "SaleItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorSale_EnvatoAccessId",
                table: "AuthorSale",
                column: "EnvatoAccessId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorSale_ItemId",
                table: "AuthorSale",
                column: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorSale");

            migrationBuilder.DropTable(
                name: "EnvatoAccess");

            migrationBuilder.DropTable(
                name: "SaleItem");
        }
    }
}
