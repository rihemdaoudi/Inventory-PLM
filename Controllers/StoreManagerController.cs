using Inventory_PLM.Data;
using Inventory_PLM.Interfaces.CategoryInterface;
using Inventory_PLM.Interfaces.ProductInterface;
using Inventory_PLM.Interfaces.PurhaseOrderInterfae;
using Inventory_PLM.Interfaces.SubCategoryInterface;
using Inventory_PLM.Interfaces.UnitOfMeasureInterface;
using Inventory_PLM.Interfaces.UserInterface;
using Inventory_PLM.Models;
using Inventory_PLM.Models.ViewModels;
using Inventory_PLM.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Inventory_PLM.Controllers
{
    public class StoreManagerController : Controller
    {
        //private readonly ApplicationDbContext _context;
        
        private readonly IProductService _productService;
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly ICategoryService _categoryService;
        private readonly ISubCategoryService _subCategoryService;
        private readonly IUnitOfMeasureService _unitOfMeasureService;

        public StoreManagerController(IProductService productService, IPurchaseOrderService purchaseOrderService, IUnitOfMeasureService unitOfMeasureService, ISubCategoryService subCategoryService, ICategoryService categoryService)
        {
           
           _productService = productService;
           _purchaseOrderService = purchaseOrderService;
            _subCategoryService = subCategoryService;
            _categoryService = categoryService;
            _unitOfMeasureService = unitOfMeasureService;
        }

        public async Task<IActionResult> DashboardStore()
        {
            var products = await _productService.GetAllProductsAsync();
            //var purchaseOrders = await _purchaseOrderService.GetAllPurchaseOrdersAsync();

            var model = new StoreDashboardViewModel
            {
                TotalProducts = products.Count(),
                //TotalPurchaseOrders = purchaseOrders.Count(),
                TotalStockValue = products.Sum(p => p.Price * p.Quantity),
                StockOverview = products.Select(p => new StockOverviewItem
                {
                    ProductName = p.Name,
                    CategoryName = p.Category.Name,
                    Quantity = p.Quantity,
                    TotalValue = p.Price * p.Quantity
                }).ToList()
            };

            return View(model);
        }

        public async Task<IActionResult> ProductManagement()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);

        }


        public async Task<IActionResult> Create()
        {

            ViewBag.CategoryID = new SelectList(await _categoryService.GetAllCategoriesAsync(), "ID", "Name");
            ViewBag.SubCategoryID = new SelectList(await _subCategoryService.GetAllSubCategoriesAsync(), "ID", "Name");
            ViewBag.UnitOfMeasureID = new SelectList(await _unitOfMeasureService.GetAllUnitsOfMeasureAsync(), "ID", "Name");
            return PartialView("_ProductFormPartial", new Product());
            //return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            
                await _productService.CreateProductAsync(product);
                return RedirectToAction(nameof(ProductManagement));
            
            ViewBag.CategoryID = new SelectList(await _categoryService.GetAllCategoriesAsync(), "ID", "Name", product.CategoryID);
            ViewBag.SubCategoryID = new SelectList(await _subCategoryService.GetAllSubCategoriesAsync(), "ID", "Name", product.SubCategoryID);
            ViewBag.UnitOfMeasureID = new SelectList(await _unitOfMeasureService.GetAllUnitsOfMeasureAsync(), "ID", "Name", product.UnitOfMeasureID);
            
            return View(product);
        }

        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Json(product);
        }

        public async Task<IActionResult> GetEditFormData(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var categories = await _categoryService.GetAllCategoriesAsync();
            var subCategories = await _subCategoryService.GetAllSubCategoriesAsync();
            var unitsOfMeasure = await _unitOfMeasureService.GetAllUnitsOfMeasureAsync();

            return Json(new
            {
                product,
                categories,
                subCategories,
                unitsOfMeasure
            });
        }


        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.CategoryID = new SelectList(await _categoryService.GetAllCategoriesAsync(), "ID", "Name", product.CategoryID);
            ViewBag.SubCategoryID = new SelectList(await _subCategoryService.GetAllSubCategoriesAsync(), "ID", "Name", product.SubCategoryID);
            ViewBag.UnitOfMeasureID = new SelectList(await _unitOfMeasureService.GetAllUnitsOfMeasureAsync(), "ID", "Name", product.UnitOfMeasureID);

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.ID)
            {
                return NotFound();
            }

            
                await _productService.UpdateProductAsync(product);
                return RedirectToAction(nameof(ProductManagement));
            
            ViewBag.CategoryID = new SelectList(await _categoryService.GetAllCategoriesAsync(), "ID", "Name", product.CategoryID);
            ViewBag.SubCategoryID = new SelectList(await _subCategoryService.GetAllSubCategoriesAsync(), "ID", "Name", product.SubCategoryID);
            ViewBag.UnitOfMeasureID = new SelectList(await _unitOfMeasureService.GetAllUnitsOfMeasureAsync(), "ID", "Name", product.UnitOfMeasureID);

            return View(product);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction(nameof(ProductManagement));
        }


       

    }
}