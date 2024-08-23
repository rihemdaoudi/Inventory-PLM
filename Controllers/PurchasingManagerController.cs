using Inventory_PLM.Data;
using Inventory_PLM.Models;
using Inventory_PLM.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static QuestPDF.Helpers.Colors;

namespace Inventory_PLM.Controllers
{
    public class PurchasingManagerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchasingManagerController (ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult DashboardPurchasing()
        {
            // Fetch total number of purchase orders
            var totalPurchaseOrders = _context.PurchaseOrders.Count();
            // Fetch trendsData 
            var trendsData = new TrendsDataModel
            {
                Labels = _context.PurchaseOrders
                       .GroupBy(po => po.PurchaseDate.Month)
                       .Select(g => new { Month = g.Key, Count = g.Count() })
                       .OrderBy(x => x.Month)
                       .Select(x => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.Month))
                       .ToList(),
                Values = _context.PurchaseOrders
                       .GroupBy(po => po.PurchaseDate.Month)
                       .Select(g => g.Count())
                       .ToList()
            };

            var inventoryLevelsData = new InventoryLevelsDataModel
            {
                Labels = _context.Products
                         .Select(p => p.Name)
                         .ToList(),
                Values = _context.Products
                         .Select(p => p.Quantity)
                         .ToList()
            };
            // Fetch recent purchase orders
            var recentPurchaseOrders = _context.PurchaseOrders
                                              .OrderByDescending(po => po.PurchaseDate)
                                              .Take(10)
                                              .Select(po => new RecentPurchaseOrderModel
                                              {
                                                  OrderNumber = po.Code,
                                                  SupplierName = po.Supplier.FirstName,
                                                  Date = po.PurchaseDate,
                                                  //Status = po.CommissioningDate
                                              })
                                              .ToList();

            var model = new PurchaseDashboardViewModel
            {
                TotalPurchaseOrders = totalPurchaseOrders,
                TrendsData = trendsData,
                InventoryLevelsData = inventoryLevelsData,
                RecentPurchaseOrders = recentPurchaseOrders

            };
           

            return View(model);
        }
        // Nouvelle action pour afficher les propositions d'achat
        public async Task<IActionResult> Proposals()
        {
            var proposals = await _context.PurchaseProposals.ToListAsync();
            return View(proposals);
        }

        // Nouvelle action pour examiner une proposition d'achat
        public async Task<IActionResult> Review(int id)
        {
            var proposal = await _context.PurchaseProposals.FindAsync(id);
            if (proposal == null)
            {
                return NotFound();
            }

            var reviewModel = new ProposalReviewViewModel
            {
                ProposalId = proposal.Id,
                Description = proposal.Description,
                Amount = proposal.Amount,
                SubmittedDate = proposal.SubmittedDate,
                SubmittedBy = proposal.SubmittedBy,
                Status = proposal.Status
            };

            return View(reviewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Review(ProposalReviewViewModel reviewModel)
        {
            if (ModelState.IsValid)
            {
                var review = new ProposalReview
                {
                    ProposalId = reviewModel.ProposalId,
                    Reviewer = User.Identity.Name,
                    Comments = reviewModel.Comments,
                    ReviewedDate = DateTime.Now,
                    Approved = reviewModel.Approved
                };

                _context.ProposalReviews.Add(review);

                var proposal = await _context.PurchaseProposals.FindAsync(reviewModel.ProposalId);
                proposal.Status = reviewModel.Approved ? "Approved" : "Rejected";

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Proposals));
            }
            return View(reviewModel);
        }


    }
}
