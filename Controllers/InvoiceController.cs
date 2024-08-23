using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventory_PLM.Data;
using Inventory_PLM.Models;
using Inventory_PLM.Enums;

namespace Inventory_PLM.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvoiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Invoice
        public async Task<IActionResult> Index(string StatusFilter)
        {
            // Commencez par récupérer toutes les factures
            var invoices = from i in _context.Invoices
                           select i;

            // Appliquer le filtre si un statut est sélectionné
            if (!string.IsNullOrEmpty(StatusFilter))
            {
                InvoiceStatus status;
                if (Enum.TryParse(StatusFilter, out status))
                {
                    invoices = invoices.Where(i => i.Status == status);
                }
            }

            // Retournez la vue avec les factures filtrées
            return View(await invoices.ToListAsync());
            //return View(await _context.Invoices.ToListAsync());
        }

        // GET: Invoice/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoice/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Invoice/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Reference,Date,Quantity,UnitPrice,VATRate,TotalAmount,AmountExclVAT,AmountInclVAT,Status,Comments")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                invoice.TotalAmount = invoice.Quantity * invoice.UnitPrice;
                invoice.AmountExclVAT = invoice.TotalAmount / (1 + invoice.VATRate / 100);
                invoice.AmountInclVAT = invoice.TotalAmount;

                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(invoice);
        }

        // GET: Invoice/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            return View(invoice);
        }

        // POST: Invoice/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Reference,Date,Quantity,UnitPrice,VATRate,TotalAmount,AmountExclVAT,AmountInclVAT,Status,Comments")] Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.Id))
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
            return View(invoice);
        }

        // GET: Invoice/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Approve(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            invoice.Status = InvoiceStatus.Approved;
            _context.Update(invoice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Reject(int id, string comments)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            invoice.Status = InvoiceStatus.Rejected;
            invoice.Comments = comments;
            _context.Update(invoice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> History()
        {
            var invoices = await _context.Invoices
                .Where(i => i.Status == InvoiceStatus.Approved || i.Status == InvoiceStatus.Rejected)
                .ToListAsync();
            return View(invoices);
        }
        public async Task<IActionResult> GeneratePdf(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            var Renderer = new HtmlToPdf();
            var pdf = Renderer.RenderHtmlAsPdf($"<h1>Invoice {invoice.Code}</h1><p>{invoice.Reference}</p>");
            var pdfBytes = pdf.BinaryData;

            return File(pdfBytes, "application/pdf", $"Invoice_{invoice.Code}.pdf");
        }






    }
}
