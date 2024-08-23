namespace Inventory_PLM.Models
{
    public class ProposalReview
    {
        public int Id { get; set; }
        public int ProposalId { get; set; }
        public string Reviewer { get; set; }
        public string Comments { get; set; }
        public DateTime ReviewedDate { get; set; }
        public bool Approved { get; set; }
    }
}
