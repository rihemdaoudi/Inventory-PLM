using System.ComponentModel.DataAnnotations;

namespace Inventory_PLM.Models
{
    public class PurchaseInvoice
    {
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Code { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal VATRate { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public decimal AmountExclTax { get; set; }

        [Required]
        public decimal AmountInclTax { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        // Navigation properties
        public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
    }
}
