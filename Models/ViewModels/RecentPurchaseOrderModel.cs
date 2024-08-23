namespace Inventory_PLM.Models.ViewModels
{
    public class RecentPurchaseOrderModel
    {
        public string OrderNumber { get; set; }
        public string SupplierName { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}
