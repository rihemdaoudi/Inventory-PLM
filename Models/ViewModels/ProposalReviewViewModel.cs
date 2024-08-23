namespace Inventory_PLM.Models.ViewModels
{
    public class ProposalReviewViewModel
    {
        public int ProposalId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string SubmittedBy { get; set; }
        public string Status { get; set; }
        public string Reviewer { get; set; }
        public string Comments { get; set; }
        public bool Approved { get; set; }
    }
}
