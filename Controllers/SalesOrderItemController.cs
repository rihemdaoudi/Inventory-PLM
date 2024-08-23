using Inventory_PLM.Data;
using Inventory_PLM.Models;
using Inventory_PLM.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace Inventory_PLM.Controllers
{
    public class SalesOrderItemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalesOrderItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SalesOrderItem
        public async Task<IActionResult> Index()
        {
            var model = new SalesOrderItemViewModel
            {
                SalesOrderItems = await _context.SalesOrderItems.ToListAsync(),
                Products = await _context.Products.ToListAsync(),
                SalesOrders = await _context.SalesOrders.ToListAsync()
            };

            return View(model);
            //var salesOrderItems = _context.SalesOrderItems
            //    .Include(s => s.Product) // Inclure les données du produit
            //    .Include(s => s.SalesOrder)
            //    .ToList();
            //return View(salesOrderItems);
        }

        // GET: SalesOrderItem/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var salesOrderItem = await _context.SalesOrderItems
                .Include(s => s.SalesOrder)
                .Include(s => s.Product)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (salesOrderItem == null)
            {
                return NotFound();
            }

            return View(salesOrderItem);
        }

        // GET: SalesOrderItem/Create
        public IActionResult Create()
        {
            ViewData["SalesOrderID"] = new SelectList(_context.SalesOrders, "ID", "Code");
            ViewData["ProductID"] = new SelectList(_context.Products, "ID", "Name");
            return View();
        }

        // POST: SalesOrderItem/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,SalesOrderID,Code,Designation,UnitPriceExclTax,UnitPriceInclTax,VAT,RequestedQuantity,DeliveredQuantity,TotalAmount")] SalesOrderItem salesOrderItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salesOrderItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SalesOrderID"] = new SelectList(_context.SalesOrders, "ID", "Code", salesOrderItem.SalesOrderID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ID", "Name", salesOrderItem.Product);
            return View(salesOrderItem);
        }

        // GET: SalesOrderItem/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var salesOrderItem = await _context.SalesOrderItems.FindAsync(id);
            if (salesOrderItem == null)
            {
                return NotFound();
            }
            ViewData["SalesOrderID"] = new SelectList(_context.SalesOrders, "ID", "Code", salesOrderItem.SalesOrderID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ID", "Name", salesOrderItem.Product);
            return View(salesOrderItem);
        }

        // POST: SalesOrderItem/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,SalesOrderID,Code,Designation,UnitPriceExclTax,UnitPriceInclTax,VAT,RequestedQuantity,DeliveredQuantity,TotalAmount")] SalesOrderItem salesOrderItem)
        {
            if (id != salesOrderItem.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salesOrderItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalesOrderItemExists(salesOrderItem.ID))
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
            ViewData["SalesOrderID"] = new SelectList(_context.SalesOrders, "ID", "Code", salesOrderItem.SalesOrderID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ID", "Name", salesOrderItem.Product);
            return View(salesOrderItem);
        }

        // GET: SalesOrderItem/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var salesOrderItem = await _context.SalesOrderItems
                .Include(s => s.SalesOrder)
                .Include(s => s.Product)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (salesOrderItem == null)
            {
                return NotFound();
            }

            return View(salesOrderItem);
        }

        // POST: SalesOrderItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salesOrderItem = await _context.SalesOrderItems.FindAsync(id);
            _context.SalesOrderItems.Remove(salesOrderItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalesOrderItemExists(int id)
        {
            return _context.SalesOrderItems.Any(e => e.ID == id);
        }
        //public IActionResult ExportToExcel()
        //{
        //    // Set the license context
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //    var salesOrderItem = _context.SalesOrderItems.Include(s => s.SalesOrderID).ToList();

        //    using (var package = new ExcelPackage())
        //    {
        //        var worksheet = package.Workbook.Worksheets.Add("SalesOrderItem");
        //        worksheet.Cells["A1"].LoadFromCollection(salesOrderItem, true);

        //        var stream = new MemoryStream();
        //        package.SaveAs(stream);
        //        stream.Position = 0;

        //        var fileName = $"SalesOrderItems_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx";
        //        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        //    }
        //}
    }
}

