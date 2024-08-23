using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventory_PLM.Data;
using Inventory_PLM.Models;
using OfficeOpenXml;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Inventory_PLM.Models.ViewModels;
using Inventory_PLM.Services;


namespace Inventory_PLM.Controllers
{
    public class PurchaseOrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PdfService _pdfService;
        private readonly ICompositeViewEngine _viewEngine;
        //private readonly IRazorViewEngine _viewEngine;
        private readonly IWebHostEnvironment _hostingEnvironment; // Use IWebHostEnvironment
        private readonly IConverter _converter;
        private readonly ITempDataProvider _tempDataProvider;

        public PurchaseOrderController(ApplicationDbContext context, ICompositeViewEngine viewEngine, //IRazorViewEngine viewEngine,
        IWebHostEnvironment hostingEnvironment, IConverter converter, ITempDataProvider tempDataProvider, PdfService pdfService)
        {
            _context = context;
            _pdfService = pdfService;
            _viewEngine = viewEngine;
            //_viewEngine = viewEngine;
            _hostingEnvironment = hostingEnvironment;
            _converter = converter;
            _tempDataProvider = tempDataProvider;
        }

        // GET: PurchaseOrder

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PurchaseOrders.Include(p => p.Supplier);
            ViewBag.Suppliers = new SelectList(_context.Suppliers, "ID", "FirstName");
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PurchaseOrder/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrder = await _context.PurchaseOrders
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }

            return View(purchaseOrder);
        }
        public async Task<IActionResult> GetPurchaseOrder(int id)
        {
            var purchase = await _context.PurchaseOrders.FindAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }
            return Json(purchase);
        }

        // GET: PurchaseOrder/Create
        public IActionResult Create()
        {
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "ID", "FirstName");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Code,Designation,PurchaseDate,CommissioningDate,SupplierID,StoreLocation,QuantityInStock,AlertQuantity")] PurchaseOrder purchaseOrder)
        {

            _context.Add(purchaseOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            ViewBag.Suppliers = new SelectList(_context.Suppliers, "ID", "FirstName", purchaseOrder.SupplierID);

            //ViewData["SupplierID"] = new SelectList(_context.Suppliers, "ID", "FirstName", purchaseOrder.SupplierID);
            return View(purchaseOrder);
        }

        // GET: PurchaseOrder/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "ID", "FirstName", purchaseOrder.SupplierID);
            return View(purchaseOrder);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Code,Designation,PurchaseDate,CommissioningDate,SupplierID,StoreLocation,QuantityInStock,AlertQuantity")] PurchaseOrder purchaseOrder)
        {
            if (id != purchaseOrder.ID)
            {
                return NotFound();
            }


            try
            {
                _context.Update(purchaseOrder);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseOrderExists(purchaseOrder.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "ID", "FirstName", purchaseOrder.SupplierID);
            return View(purchaseOrder);
        }

        // GET: PurchaseOrder/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrder = await _context.PurchaseOrders
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }

            return View(purchaseOrder);
        }

        // POST: PurchaseOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);
            if (purchaseOrder != null)
            {
                _context.PurchaseOrders.Remove(purchaseOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseOrderExists(int id)
        {
            return _context.PurchaseOrders.Any(e => e.ID == id);
        }

        public IActionResult ExportToExcel()
        {
            // Set the license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; 
            var purchaseOrders = _context.PurchaseOrders.Include(p => p.Supplier).ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("PurchaseOrders");
                worksheet.Cells["A1"].LoadFromCollection(purchaseOrders, true);

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                var fileName = $"PurchaseOrders_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        public IActionResult ExportToPdf()
        {
            // Get the data to be included in the PDF
            var purchaseOrders = _context.PurchaseOrders.Include(p => p.Supplier).ToList();

            // Convert the data to an HTML string 
            var htmlContent = RenderViewToString("~/Views/PurchaseOrder/ExportToPdf.cshtml", purchaseOrders);
            // Generate the PDF 
            var pdfBytes = _pdfService.GeneratePdf(htmlContent);

            // Return the PDF file
            return File(pdfBytes, "application/pdf", "PurchaseOrders.pdf");

        }
        private string RenderViewToString(string viewName, object model)
        {
            var viewResult = _viewEngine.GetView(null, viewName, false);

            if (!viewResult.Success)
            {
                throw new InvalidOperationException($"Couldn't find view '{viewName}'.");
            }

            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };

            using (var sw = new StringWriter())
            {
                var viewContext = new ViewContext(
                    new ActionContext
                    {
                        HttpContext = HttpContext,
                        RouteData = RouteData,
                        ActionDescriptor = ControllerContext.ActionDescriptor
                    },
                    viewResult.View,
                    viewData,
                    new TempDataDictionary(HttpContext, _tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                viewResult.View.RenderAsync(viewContext).Wait();
                return sw.ToString();
            }
        }
    }

}
