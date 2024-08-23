using System.ComponentModel.DataAnnotations;

namespace Inventory_PLM.Models
{
    public class PurchaseOrderItem
    {
        public int ID { get; set; }

        [Required]
        public int PurchaseOrderID { get; set; }

        [Required]
        [StringLength(100)]
        public string Code { get; set; }

        [Required]
        [StringLength(200)]
        public string Designation { get; set; }

        [Required]
        public int ProductID { get; set; } 
       // [Required]
       // public int CategoryID { get; set; }

        [Required]
        public decimal UnitPriceExclTax { get; set; }

        [Required]
        public decimal UnitPriceInclTax { get; set; }

        [Required]
        public decimal VAT { get; set; }

        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal TotalAmount { get; set; } // Montant total (UnitPriceInclTax * Quantity)
        // Navigation properties
        public PurchaseOrder PurchaseOrder { get; set; }
        public Product Product { get; set; }  

        //public Category Category { get; set; }
    }
}
