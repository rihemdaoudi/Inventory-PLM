using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory_PLM.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchaseInvoices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PurchaseInvoiceID",
                table: "PurchaseOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PurchaseInvoices",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    VATRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountExclTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountInclTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseInvoices", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PurchaseInvoiceID",
                table: "PurchaseOrders",
                column: "PurchaseInvoiceID");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_PurchaseInvoices_PurchaseInvoiceID",
                table: "PurchaseOrders",
                column: "PurchaseInvoiceID",
                principalTable: "PurchaseInvoices",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_PurchaseInvoices_PurchaseInvoiceID",
                table: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "PurchaseInvoices");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_PurchaseInvoiceID",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "PurchaseInvoiceID",
                table: "PurchaseOrders");
        }
    }
}
