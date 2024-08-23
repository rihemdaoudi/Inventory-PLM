using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventory_PLM.Data;
using Inventory_PLM.Models;

namespace Inventory_PLM.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: Product
        public async Task<IActionResult> Index()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .Include(p => p.SubCategory)
                .Include(p => p.UnitOfMeasure);

            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Name");
            ViewData["SubCategoryID"] = new SelectList(_context.SubCategories, "ID", "Name");
            ViewData["UnitOfMeasureID"] = new SelectList(_context.UnitOfMeasures, "ID", "Name");
            return View(await products.ToListAsync());
        }


        //// GET: Product/GetProduct/5
        //public async Task<IActionResult> GetProduct(int id)
        //{
        //    var product = await _context.Products
        //        .Include(p => p.Category)
        //        .Include(p => p.SubCategory)
        //        .Include(p => p.UnitOfMeasure)
        //        .FirstOrDefaultAsync(p => p.ID == id);

        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return Json(new
        //    {
        //        id = product.ID,
        //        code = product.Code,
        //        name = product.Name,
        //        description = product.Description,
        //        price = product.Price,
        //        quantity = product.Quantity,
        //        categoryID = product.CategoryID,
        //        subCategoryID = product.SubCategoryID,
        //        unitOfMeasureID = product.UnitOfMeasureID,
        //         currentStock = product.CurrentStock,
        //        minimumStockLevel = product.MinimumStockLevel,
        //        isActive = product.IsActive
        //    });
        //}
        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.SubCategory)
                .Include(p => p.UnitOfMeasure)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Name");
            ViewData["SubCategoryID"] = new SelectList(_context.SubCategories, "ID", "Name");
            ViewData["UnitOfMeasureID"] = new SelectList(_context.UnitOfMeasures, "ID", "Name");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,CategoryID,SubCategoryID,UnitOfMeasureID,Code,Description,Price,Quantity,CurrentStock,MinimumStockLevel,IsActive")] Product product)
        {
            

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Name", product.CategoryID);
            ViewData["SubCategoryID"] = new SelectList(_context.SubCategories, "ID", "Name", product.SubCategoryID);
            ViewData["UnitOfMeasureID"] = new SelectList(_context.UnitOfMeasures, "ID", "Name", product.UnitOfMeasureID);
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Name", product.CategoryID);
            ViewData["SubCategoryID"] = new SelectList(_context.SubCategories, "ID", "Name", product.SubCategoryID);
            ViewData["UnitOfMeasureID"] = new SelectList(_context.UnitOfMeasures, "ID", "Name", product.UnitOfMeasureID);
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,CategoryID,SubCategoryID,UnitOfMeasureID,Code,Description,Price,Quantity,CurrentStock,MinimumStockLevel,IsActive")] Product product)
        {
            if (id != product.ID)
            {
                return NotFound();
            }

                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                
                  
                }
            return RedirectToAction(nameof(Index));
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Name", product.CategoryID);
            ViewData["SubCategoryID"] = new SelectList(_context.SubCategories, "ID", "Name", product.SubCategoryID);
            ViewData["UnitOfMeasureID"] = new SelectList(_context.UnitOfMeasures, "ID", "Name", product.UnitOfMeasureID);
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.SubCategory)
                .Include(p => p.UnitOfMeasure)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


       
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ID == id);
        }
    }
}
