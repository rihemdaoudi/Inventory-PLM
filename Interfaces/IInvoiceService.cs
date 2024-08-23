using Inventory_PLM.Models;
using System.Threading.Tasks;

namespace Inventory_PLM.Interfaces
{
    public interface IInvoiceService
    {
        Task<IEnumerable<Invoice>> GetAllInvoicesAsync();
        Task<Invoice> GetInvoiceByIdAsync(int id);
        Task AddInvoiceAsync(Invoice invoice);
        Task UpdateInvoiceAsync(Invoice invoice);
        Task DeleteInvoiceAsync(int id);
        Task ApproveInvoiceAsync(int id, string comments);
        Task RejectInvoiceAsync(int id, string comments);
    }
}
