using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory_PLM.Migrations
{
    /// <inheritdoc />
    public partial class AddNewItemsToPurchaseOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderItems_Categories_CategoryID",
                table: "PurchaseOrderItems");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderItems_CategoryID",
                table: "PurchaseOrderItems");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "PurchaseOrderItems");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "PurchaseOrderItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "PurchaseOrderItems");

            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "PurchaseOrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_CategoryID",
                table: "PurchaseOrderItems",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderItems_Categories_CategoryID",
                table: "PurchaseOrderItems",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
