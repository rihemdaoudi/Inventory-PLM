using System.ComponentModel.DataAnnotations;

namespace Inventory_PLM.Models
{
    public class SalesOrderItem
    {
        public int ID { get; set; }

        [Required]
        public int SalesOrderID { get; set; }

        [Required]
        [StringLength(100)]
        public string Code { get; set; }

        [Required]
        [StringLength(200)]
        public string Designation { get; set; }

        [Required]
        public decimal UnitPriceExclTax { get; set; }

        [Required]
        public decimal UnitPriceInclTax { get; set; }

        [Required]
        public decimal VAT { get; set; }

        [Required]
        public int RequestedQuantity { get; set; }

        public int DeliveredQuantity { get; set; } // Quantité livrée (peut différer de RequestedQuantity)

        [Required]
        public decimal TotalAmount { get; set; } // Montant total (UnitPriceInclTax * RequestedQuantity)
        //[Required]
        //public decimal TotalPrice { get; set; }

        // Navigation properties
        public SalesOrder SalesOrder { get; set; }
        public Product Product { get; set; }
        
    }
}
