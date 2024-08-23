using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory_PLM.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAttributsInvoiceModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Invoices",
                newName: "VATRate");

            migrationBuilder.RenameColumn(
                name: "TauxTVA",
                table: "Invoices",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Invoices",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "AmountTTC",
                table: "Invoices",
                newName: "AmountInclVAT");

            migrationBuilder.RenameColumn(
                name: "AmountHT",
                table: "Invoices",
                newName: "AmountExclVAT");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "SalesOrders",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentStatus",
                table: "SalesOrders",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Invoices",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Comments",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VATRate",
                table: "Invoices",
                newName: "TotalPrice");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "Invoices",
                newName: "TauxTVA");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Invoices",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "AmountInclVAT",
                table: "Invoices",
                newName: "AmountTTC");

            migrationBuilder.RenameColumn(
                name: "AmountExclVAT",
                table: "Invoices",
                newName: "AmountHT");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "SalesOrders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentStatus",
                table: "SalesOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Comments",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
