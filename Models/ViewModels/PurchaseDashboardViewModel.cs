namespace Inventory_PLM.Models.ViewModels
{
    public class PurchaseDashboardViewModel
    {
        public int TotalPurchaseOrders { get; set; }
        public TrendsDataModel TrendsData { get; set; }
        public InventoryLevelsDataModel InventoryLevelsData { get; set; }
        public List<RecentPurchaseOrderModel> RecentPurchaseOrders { get; set; }

    }
}
