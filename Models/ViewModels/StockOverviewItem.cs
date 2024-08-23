namespace Inventory_PLM.Models.ViewModels
{
    public class StockOverviewItem
    {
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalValue { get; set; }
        public int ProductCount { get; set; }

    }
}
