using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inventory_PLM.Models.ViewModels
{
    public class InvoiceFilterViewModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public IEnumerable<Invoice> Invoices { get; set; }
        public List<SelectListItem> StatusList { get; set; }

        public InvoiceFilterViewModel()
        {
            StatusList = new List<SelectListItem>
            {
            new SelectListItem { Value = "", Text = "Tous" },
            new SelectListItem { Value = "Pending", Text = "Pending" },
            new SelectListItem { Value = "Approved", Text = "Approved" },
            new SelectListItem { Value = "Rejected", Text = "Rejected" }
            };
        }
       
       
    }

}
