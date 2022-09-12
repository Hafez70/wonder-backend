using Microsoft.EntityFrameworkCore.Migrations;

namespace WTLicVerify.Migrations
{
    public partial class dbkeyschanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorSale_EnvatoAccess_EnvatoAccessId",
                table: "AuthorSale");

            migrationBuilder.DropIndex(
                name: "IX_AuthorSale_EnvatoAccessId",
                table: "AuthorSale");

            migrationBuilder.DropColumn(
                name: "EnvatoAccessId",
                table: "AuthorSale");

            migrationBuilder.AddColumn<long>(
                name: "authorSaleId",
                table: "EnvatoAccess",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_EnvatoAccess_authorSaleId",
                table: "EnvatoAccess",
                column: "authorSaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_EnvatoAccess_AuthorSale_authorSaleId",
                table: "EnvatoAccess",
                column: "authorSaleId",
                principalTable: "AuthorSale",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnvatoAccess_AuthorSale_authorSaleId",
                table: "EnvatoAccess");

            migrationBuilder.DropIndex(
                name: "IX_EnvatoAccess_authorSaleId",
                table: "EnvatoAccess");

            migrationBuilder.DropColumn(
                name: "authorSaleId",
                table: "EnvatoAccess");

            migrationBuilder.AddColumn<long>(
                name: "EnvatoAccessId",
                table: "AuthorSale",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthorSale_EnvatoAccessId",
                table: "AuthorSale",
                column: "EnvatoAccessId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorSale_EnvatoAccess_EnvatoAccessId",
                table: "AuthorSale",
                column: "EnvatoAccessId",
                principalTable: "EnvatoAccess",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
