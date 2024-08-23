namespace Inventory_PLM.Models
{
    public class PurchaseProposal
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string SubmittedBy { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected
    }
}
