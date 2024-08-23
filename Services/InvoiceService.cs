//using Inventory_PLM.Data;
//using Inventory_PLM.Interfaces;
//using Inventory_PLM.Models;
//using Microsoft.EntityFrameworkCore;

//namespace Inventory_PLM.Services
//{
//    public class InvoiceService : IInvoiceService
//    {
//        private readonly ApplicationDbContext _context;

//        public InvoiceService(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync()
//        {
//            return await _context.Invoices.ToListAsync();
//        }

//        public async Task<Invoice> GetInvoiceByIdAsync(int id)
//        {
//            return await _context.Invoices.FindAsync(id);
//        }

//        public async Task AddInvoiceAsync(Invoice invoice)
//        {
//            _context.Invoices.Add(invoice);
//            await _context.SaveChangesAsync();
//        }

//        public async Task UpdateInvoiceAsync(Invoice invoice)
//        {
//            _context.Invoices.Update(invoice);
//            await _context.SaveChangesAsync();
//        }

//        public async Task DeleteInvoiceAsync(int id)
//        {
//            var invoice = await _context.Invoices.FindAsync(id);
//            if (invoice != null)
//            {
//                _context.Invoices.Remove(invoice);
//                await _context.SaveChangesAsync();
//            }
//        }

//        public async Task ApproveInvoiceAsync(int id, string comments)
//        {
//            var invoice = await _context.Invoices.FindAsync(id);
//            if (invoice != null)
//            {
//                invoice.Status = "Approved";
//                invoice.Comments = comments;
//                _context.Invoices.Update(invoice);
//                await _context.SaveChangesAsync();
//            }
//        }

//        public async Task RejectInvoiceAsync(int id, string comments)
//        {
//            var invoice = await _context.Invoices.FindAsync(id);
//            if (invoice != null)
//            {
//                invoice.Status = "Rejected";
//                invoice.Comments = comments;
//                _context.Invoices.Update(invoice);
//                await _context.SaveChangesAsync();
//            }
//        }
//    }
//}
