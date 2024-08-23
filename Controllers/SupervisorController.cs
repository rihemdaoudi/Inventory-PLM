using Inventory_PLM.Data;
using Inventory_PLM.Enums;
using Inventory_PLM.Interfaces.UserInterface;
using Inventory_PLM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Inventory_PLM.Controllers
{
    public class SupervisorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupervisorController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult DashboardSupervisor()
        {
            return View();
        }
        // Liste des commandes à valider
        public async Task<IActionResult> Index()
        {
            var salesOrders = await _context.SalesOrders
                .Where(so => so.Status == OrderStatus.Pending)
                .Include(so => so.Customer)
                .ToListAsync();

            return View(salesOrders);
        }

        // GET: Supervisor/Approve/5
        public async Task<IActionResult> Approve(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrder = await _context.SalesOrders
                .Include(so => so.Customer)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (salesOrder == null)
            {
                return NotFound();
            }

            return View(salesOrder);
        }

        // POST: Supervisor/Approve/5
        [HttpPost, ActionName("Approve")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveConfirmed(int id)
        {
            var salesOrder = await _context.SalesOrders
                               .Include(so => so.Items)
                               .FirstOrDefaultAsync(so => so.ID == id);

            if (salesOrder == null)
            {
                return NotFound();
            }

            try
            {
                salesOrder.Status = OrderStatus.Validated;
                _context.Update(salesOrder);
                await _context.SaveChangesAsync();

                // Mise à jour du stock
                await UpdateStockAsync(salesOrder);

                // Génération de la facture
                await GenerateInvoiceAsync(salesOrder);
            }
            catch (InvalidOperationException ex)
            {
                // Ajout de l'erreur au ModelState pour l'afficher dans la vue
                ModelState.AddModelError(string.Empty, ex.Message);
                // Récupérer à nouveau la liste des commandes pour les afficher
                var orders = await _context.SalesOrders
                    .Include(so => so.Customer)
                    .ToListAsync();

                // Retourner la vue d'approbation avec le message d'erreur
                return View("Approve", salesOrder);
            }

            return RedirectToAction(nameof(Index));
            //var salesOrder = await _context.SalesOrders.FindAsync(id);
            //if (salesOrder != null)
            //{
            //    salesOrder.Status = OrderStatus.Validated;
            //    _context.Update(salesOrder);
            //    await _context.SaveChangesAsync();
            //    // Mise à jour du stock
            //    await UpdateStockAsync(salesOrder);

            //    // Génération de la facture
            //    await GenerateInvoiceAsync(salesOrder);
            //}
            //return RedirectToAction(nameof(Index));
        }

        // GET: Supervisor/Reject/5
        public async Task<IActionResult> Reject(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrder = await _context.SalesOrders
                .Include(so => so.Customer)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (salesOrder == null)
            {
                return NotFound();
            }

            return View(salesOrder);
        }

        // POST: Supervisor/Reject/5
        [HttpPost, ActionName("Reject")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectConfirmed(int id)
        {
            var salesOrder = await _context.SalesOrders.FindAsync(id);
            if (salesOrder != null)
            {
                salesOrder.Status = OrderStatus.Cancelled;
                _context.Update(salesOrder);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task UpdateStockAsync(SalesOrder salesOrder)
        {
            foreach (var item in salesOrder.Items)
            {
                var product = await _context.Products.FindAsync(item.Product.ID);
                if (product != null)
                {
                    product.Quantity -= item.DeliveredQuantity; 
                    _context.Update(product);
                }
            }
            await _context.SaveChangesAsync();
        }
        private async Task GenerateInvoiceAsync(SalesOrder salesOrder)
        {
            if (!salesOrder.Items.Any())
            {
                // Handle the case where there are no items in the order
                throw new InvalidOperationException("The sales order has no items., the invoice cannot be generated.");
               
            }

            var invoice = new Invoice
            {
                Code = "INV-" + DateTime.Now.Ticks, // Generate a unique code
                Reference = salesOrder.Code, // Reference the SalesOrder
                Date = DateTime.Now,
                Quantity = salesOrder.Items.Sum(i => i.DeliveredQuantity), // Total delivered quantity
                VATRate = salesOrder.Items.First().VAT, // Assuming VAT is consistent across items
                UnitPrice = salesOrder.Items.Sum(i => i.UnitPriceExclTax * i.DeliveredQuantity), // Total price before VAT
                AmountExclVAT = salesOrder.Items.Sum(i => i.UnitPriceExclTax * i.DeliveredQuantity), // Total amount before tax
                AmountInclVAT = salesOrder.Items.Sum(i => i.TotalAmount), // Total amount including tax
                TotalAmount = salesOrder.Items.Sum(i => i.TotalAmount), // Assuming no additional fees
                Status = InvoiceStatus.Pending, // Initial status
                Comments = "Invoice generated automatically."
            };

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
        }


    }

}

