using Inventory_PLM.Data;
using Inventory_PLM.Interfaces.PurhaseOrderInterfae;
using Inventory_PLM.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory_PLM.Services.PurchaseOrderService
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly ApplicationDbContext _context;

        public PurchaseOrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrdersAsync()
        {
            return await _context.PurchaseOrders.Include(po => po.Supplier).ToListAsync();
        }

        public async Task<PurchaseOrder> GetPurchaseOrderByIdAsync(int id)
        {
            return await _context.PurchaseOrders.Include(po => po.Supplier)
                                                .FirstOrDefaultAsync(po => po.ID == id);
        }

        public async Task<bool> CreatePurchaseOrderAsync(PurchaseOrder purchaseOrder)
        {
            _context.PurchaseOrders.Add(purchaseOrder);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder)
        {
            _context.PurchaseOrders.Update(purchaseOrder);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeletePurchaseOrderAsync(int id)
        {
            var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);
            if (purchaseOrder == null) return false;

            _context.PurchaseOrders.Remove(purchaseOrder);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
