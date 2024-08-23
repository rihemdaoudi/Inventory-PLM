using Inventory_PLM.Models;

namespace Inventory_PLM.Interfaces.PurhaseOrderInterfae
{
        public interface IPurchaseOrderService
        {
            Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrdersAsync();
            Task<PurchaseOrder> GetPurchaseOrderByIdAsync(int id);
            Task<bool> CreatePurchaseOrderAsync(PurchaseOrder purchaseOrder);
            Task<bool> UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder);
            Task<bool> DeletePurchaseOrderAsync(int id);
        }

}
