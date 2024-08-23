namespace Inventory_PLM.Models.ViewModels
{
    public class StoreDashboardViewModel
    {
        public int TotalProducts { get; set; }
        public int TotalPurchaseOrders { get; set; }
        public decimal TotalStockValue { get; set; }
        public List<StockOverviewItem> StockOverview { get; set; }
    }
}
