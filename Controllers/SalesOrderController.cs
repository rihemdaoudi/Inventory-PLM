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
using Inventory_PLM.Models.ViewModels;

namespace Inventory_PLM.Controllers
{
    public class SalesOrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalesOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SalesOrder
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SalesOrders.Include(s => s.Customer);

            ViewBag.Customers = new SelectList(_context.Customers, "ID", "FirstName");

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SalesOrder/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrder = await _context.SalesOrders
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (salesOrder == null)
            {
                return NotFound();
            }
            var status = salesOrder.Status.HasValue ? salesOrder.Status.Value : OrderStatus.Pending;
            var paymentStatus = salesOrder.PaymentStatus.HasValue ? salesOrder.PaymentStatus.Value : PaymentStatus.Unpaid;
            // Passez les valeurs vérifiées à la vue
            var viewModel = new SalesOrderDetailsViewModel
            {
                ID = salesOrder.ID,
                Code = salesOrder.Code,
                Designation = salesOrder.Designation,
                SaleDate = salesOrder.SaleDate,
                CustomerID = salesOrder.CustomerID,
                StoreLocation = salesOrder.StoreLocation,
                TotalAmount = salesOrder.TotalAmount,
                Status = status,
                PaymentStatus = paymentStatus,
                ShippingDate = salesOrder.ShippingDate,
                Customer = salesOrder.Customer,
                Items = salesOrder.Items
            };

            return View(viewModel);
        }

        // GET: SalesOrder/Create
        public IActionResult Create()
        {
            ViewData["CustomerID"] = new SelectList(_context.Customers, "ID", "FirstName");
            return View();
        }

        // POST: SalesOrder/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SalesOrderViewModel viewModel)
        {
            //if (ModelState.IsValid)
            //{
                // Créer l'objet SalesOrder
                var salesOrder = new SalesOrder
                {
                    Code = viewModel.Code,
                    Designation = viewModel.Designation,
                    SaleDate = viewModel.SaleDate,
                    CustomerID = viewModel.CustomerID,
                    StoreLocation = viewModel.StoreLocation,
                    Status = OrderStatus.Pending,
                    PaymentStatus = PaymentStatus.Unpaid,
                    Items = new List<SalesOrderItem>()
                };

                // Ajout des SalesOrderItems
                foreach (var item in viewModel.Items)
                {
                    var orderItem = new SalesOrderItem
                    {
                        Code = item.Code,
                        Designation = item.Designation,
                        UnitPriceExclTax = item.UnitPriceExclTax,
                        UnitPriceInclTax = item.UnitPriceInclTax,
                        VAT = item.VAT,
                        RequestedQuantity = item.RequestedQuantity,
                        DeliveredQuantity = 0, // Initialement 0
                        TotalAmount = item.UnitPriceInclTax * item.RequestedQuantity
                    };

                    salesOrder.Items.Add(orderItem);
                }

                // Calcul du montant total
                salesOrder.TotalAmount = salesOrder.Items.Sum(i => i.TotalAmount);

                // Ajout de la commande à la base de données
                _context.Add(salesOrder);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    // Gérer l'erreur, par exemple, enregistrer le message dans les logs
                    ModelState.AddModelError(string.Empty, "Une erreur s'est produite lors de la création de la commande.");
                    ViewData["CustomerID"] = new SelectList(_context.Customers, "ID", "FirstName", viewModel.CustomerID);
                    return View(viewModel);
                }

                return RedirectToAction(nameof(Index));
            //}

            // Si le modèle n'est pas valide, retourner à la vue avec les erreurs
            ViewData["CustomerID"] = new SelectList(_context.Customers, "ID", "FirstName", viewModel.CustomerID);
            return View(viewModel);
        }
        //public async Task<IActionResult> Create([Bind("ID,Code,Designation,SaleDate,CustomerID,StoreLocation,TotalAmount")] SalesOrder salesOrder)
        //{
        //    salesOrder.Status = OrderStatus.Pending;

        //        _context.Add(salesOrder);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));

        //    ViewData["CustomerID"] = new SelectList(_context.Customers, "ID", "FirstName", salesOrder.CustomerID);
        //    return View(salesOrder);
        //}

        // GET: SalesOrder/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var salesOrder = await _context.SalesOrders
         .Include(s => s.Items) // Inclure les items de la commande
         .FirstOrDefaultAsync(m => m.ID == id);

            if (salesOrder == null)
            {
                return NotFound();
            }

            // Préparer le ViewModel avec les données existantes
            var viewModel = new SalesOrderViewModel
            {
                ID = salesOrder.ID,
                Code = salesOrder.Code,
                Designation = salesOrder.Designation,
                SaleDate = salesOrder.SaleDate,
                CustomerID = salesOrder.CustomerID,
                StoreLocation = salesOrder.StoreLocation,
                TotalAmount = salesOrder.TotalAmount,
                Status = salesOrder.Status,
                PaymentStatus = salesOrder.PaymentStatus,
                ShippingDate = salesOrder.ShippingDate,
                Items = salesOrder.Items.Select(i => new SalesOrderItemViewModel
                {
                    ID = i.ID,
                    Code = i.Code,
                    Designation = i.Designation,
                    UnitPriceExclTax = i.UnitPriceExclTax,
                    UnitPriceInclTax = i.UnitPriceInclTax,
                    VAT = i.VAT,
                    RequestedQuantity = i.RequestedQuantity,
                    DeliveredQuantity = i.DeliveredQuantity,
                    TotalAmount = i.TotalAmount
                }).ToList()
            };

            ViewData["CustomerID"] = new SelectList(_context.Customers, "ID", "FirstName", salesOrder.CustomerID);
            return View(viewModel);

            //var salesOrder = await _context.SalesOrders.FindAsync(id);
            //if (salesOrder == null)
            //{
            //    return NotFound();
            //}
            //ViewData["CustomerID"] = new SelectList(_context.Customers, "ID", "FirstName", salesOrder.CustomerID);
            //return View(salesOrder);
        }

        // POST: SalesOrder/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SalesOrderViewModel viewModel /*[Bind("ID,Code,Designation,SaleDate,CustomerID,StoreLocation,TotalAmount,Status")] SalesOrder salesOrder*/)
        {
            if (id != viewModel.ID)
            {
                return NotFound();
            }

           // if (ModelState.IsValid)
           // {
                try
                {
                    // Récupérer la commande existante de la base de données
                    var salesOrder = await _context.SalesOrders
                        .Include(s => s.Items)
                        .FirstOrDefaultAsync(m => m.ID == id);

                    if (salesOrder == null)
                    {
                        return NotFound();
                    }

                    // Mettre à jour les propriétés de la commande
                    salesOrder.Code = viewModel.Code;
                    salesOrder.Designation = viewModel.Designation;
                    salesOrder.SaleDate = viewModel.SaleDate;
                    salesOrder.CustomerID = viewModel.CustomerID;
                    salesOrder.StoreLocation = viewModel.StoreLocation;
                    salesOrder.Status = viewModel.Status;
                    salesOrder.PaymentStatus = viewModel.PaymentStatus;
                    salesOrder.ShippingDate = viewModel.ShippingDate;

                    // Mettre à jour ou ajouter les SalesOrderItems
                    foreach (var item in viewModel.Items)
                    {
                        var existingItem = salesOrder.Items.FirstOrDefault(i => i.ID == item.ID);
                        if (existingItem != null)
                        {
                            // Mettre à jour l'article existant
                            existingItem.Code = item.Code;
                            existingItem.Designation = item.Designation;
                            existingItem.UnitPriceExclTax = item.UnitPriceExclTax;
                            existingItem.UnitPriceInclTax = item.UnitPriceInclTax;
                            existingItem.VAT = item.VAT;
                            existingItem.RequestedQuantity = item.RequestedQuantity;
                            existingItem.DeliveredQuantity = item.DeliveredQuantity;
                            existingItem.TotalAmount = item.UnitPriceInclTax * item.RequestedQuantity;
                        }
                        else
                        {
                            // Ajouter un nouvel article
                            var newItem = new SalesOrderItem
                            {
                                Code = item.Code,
                                Designation = item.Designation,
                                UnitPriceExclTax = item.UnitPriceExclTax,
                                UnitPriceInclTax = item.UnitPriceInclTax,
                                VAT = item.VAT,
                                RequestedQuantity = item.RequestedQuantity,
                                DeliveredQuantity = item.DeliveredQuantity,
                                TotalAmount = item.UnitPriceInclTax * item.RequestedQuantity
                            };
                            salesOrder.Items.Add(newItem);
                        }
                    }

                    // Recalculer le montant total
                    salesOrder.TotalAmount = salesOrder.Items.Sum(i => i.TotalAmount);

                    // Mettre à jour la commande dans la base de données
                    _context.Update(salesOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalesOrderExists(viewModel.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
           // }

            ViewData["CustomerID"] = new SelectList(_context.Customers, "ID", "FirstName", viewModel.CustomerID);
            return View(viewModel);
            //    try
            //    {
            //        _context.Update(salesOrder);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!SalesOrderExists(salesOrder.ID))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));

            //ViewData["CustomerID"] = new SelectList(_context.Customers, "ID", "FirstName", salesOrder.CustomerID);
            //return View(salesOrder);
        }

        // GET: SalesOrder/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrder = await _context.SalesOrders
                .Include(s => s.Items)
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (salesOrder == null)
            {
                return NotFound();
            }
            // Préparer le ViewModel pour la confirmation de suppression
            var viewModel = new SalesOrderViewModel
            {
                ID = salesOrder.ID,
                Code = salesOrder.Code,
                Designation = salesOrder.Designation,
                SaleDate = salesOrder.SaleDate,
                CustomerID = salesOrder.CustomerID,
                StoreLocation = salesOrder.StoreLocation,
                TotalAmount = salesOrder.TotalAmount,
                Status = salesOrder.Status,
                PaymentStatus = salesOrder.PaymentStatus,
                ShippingDate = salesOrder.ShippingDate,
                Customer = salesOrder.Customer,
                Items = salesOrder.Items.Select(i => new SalesOrderItemViewModel
                {
                    ID = i.ID,
                    Code = i.Code,
                    Designation = i.Designation,
                    UnitPriceExclTax = i.UnitPriceExclTax,
                    UnitPriceInclTax = i.UnitPriceInclTax,
                    VAT = i.VAT,
                    RequestedQuantity = i.RequestedQuantity,
                    DeliveredQuantity = i.DeliveredQuantity,
                    TotalAmount = i.TotalAmount
                }).ToList()
            };

            return View(viewModel);
        }

        // POST: SalesOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salesOrder = await _context.SalesOrders
                .Include(s => s.Items)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (salesOrder == null)
            {
                return NotFound();
            }

            try
            {
                // Supprimer d'abord les articles associés
                _context.SalesOrderItems.RemoveRange(salesOrder.Items);

                // Puis supprimer la commande
                _context.SalesOrders.Remove(salesOrder);

                // Sauvegarder les changements dans la base de données
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Gérer les erreurs possibles et loguer l'erreur si nécessaire
                ModelState.AddModelError("", "Une erreur est survenue lors de la suppression de la commande. " + ex.Message);

                // Retourner à la vue avec l'objet salesOrder pour plus de détails
                return View(salesOrder);
            }
        
          //if (salesOrder != null)
           //{
          //    _context.SalesOrders.Remove(salesOrder);
          //}

           //await _context.SaveChangesAsync();
           //return RedirectToAction(nameof(Index));
        }
    // GET: SalesOrder/Validate/5
    public async Task<IActionResult> Validate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrder = await _context.SalesOrders.FindAsync(id);
            if (salesOrder == null)
            {
                return NotFound();
            }

            return View(salesOrder);
        }

        // POST: SalesOrder/Validate/5
        [HttpPost, ActionName("Validate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValidateConfirmed(int id)
        {
            var salesOrder = await _context.SalesOrders.FindAsync(id);
            if (salesOrder != null)
            {
                // MAJ to  "Validate"
                salesOrder.Status = OrderStatus.Validated;
                _context.Update(salesOrder);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SalesOrderExists(int id)
        {
            return _context.SalesOrders.Any(e => e.ID == id);
        }
    }
}
