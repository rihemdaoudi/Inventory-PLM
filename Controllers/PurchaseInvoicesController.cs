using Inventory_PLM.Data;
using Inventory_PLM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventory_PLM.Controllers
{
    public class PurchaseInvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchaseInvoicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PurchaseInvoices
        public async Task<IActionResult> Index()
        {
            return View(await _context.PurchaseInvoices.ToListAsync());
        }
        [HttpGet]
        public IActionResult GetInvoice(int id)
        {
            var invoice = _context.PurchaseInvoices.Find(id);
            if (invoice == null)
            {
                return NotFound();
            }
            return Json(invoice);
        }


        // GET: PurchaseInvoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseInvoice = await _context.PurchaseInvoices
                .FirstOrDefaultAsync(m => m.ID == id);
            if (purchaseInvoice == null)
            {
                return NotFound();
            }

            return View(purchaseInvoice);
        }

        // GET: PurchaseInvoices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PurchaseInvoices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Code,Date,Quantity,VATRates,Price,AmountExclTax,AmountInclTax,TotalPrice")] PurchaseInvoice purchaseInvoice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchaseInvoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(purchaseInvoice);
        }

        // GET: PurchaseInvoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseInvoice = await _context.PurchaseInvoices.FindAsync(id);
            if (purchaseInvoice == null)
            {
                return NotFound();
            }
            return View(purchaseInvoice);
        }

        // POST: PurchaseInvoices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Code,Date,Quantity,VATRates,Price,AmountExclTax,AmountInclTax,TotalPrice")] PurchaseInvoice purchaseInvoice)
        {
            if (id != purchaseInvoice.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchaseInvoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseInvoiceExists(purchaseInvoice.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(purchaseInvoice);
        }

        // GET: PurchaseInvoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseInvoice = await _context.PurchaseInvoices
                .FirstOrDefaultAsync(m => m.ID == id);
            if (purchaseInvoice == null)
            {
                return NotFound();
            }

            return View(purchaseInvoice);
        }

        // POST: PurchaseInvoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchaseInvoice = await _context.PurchaseInvoices.FindAsync(id);
            _context.PurchaseInvoices.Remove(purchaseInvoice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseInvoiceExists(int id)
        {
            return _context.PurchaseInvoices.Any(e => e.ID == id);
        }
    }
}
