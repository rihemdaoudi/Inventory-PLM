using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory_PLM.Migrations
{
    /// <inheritdoc />
    public partial class AddNewItemsToSalesOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantityInStock",
                table: "PurchaseOrders");

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "SalesOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ShippingDate",
                table: "SalesOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "SalesOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "SalesOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "DeliveredQuantity",
                table: "SalesOrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductID",
                table: "SalesOrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "SalesOrderItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderItems_ProductID",
                table: "SalesOrderItems",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrderItems_Products_ProductID",
                table: "SalesOrderItems",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrderItems_Products_ProductID",
                table: "SalesOrderItems");

            migrationBuilder.DropIndex(
                name: "IX_SalesOrderItems_ProductID",
                table: "SalesOrderItems");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "ShippingDate",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "DeliveredQuantity",
                table: "SalesOrderItems");

            migrationBuilder.DropColumn(
                name: "ProductID",
                table: "SalesOrderItems");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "SalesOrderItems");

            migrationBuilder.AddColumn<int>(
                name: "QuantityInStock",
                table: "PurchaseOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
