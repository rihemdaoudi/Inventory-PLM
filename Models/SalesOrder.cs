using Inventory_PLM.Enums;
using System.ComponentModel.DataAnnotations;

namespace Inventory_PLM.Models
{
    public class SalesOrder
    {
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Code { get; set; }

        [Required]
        [StringLength(200)]
        public string Designation { get; set; }

        [Required]
        public DateTime SaleDate { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [Required]
        [StringLength(100)]
        public string StoreLocation { get; set; }

        [Required]
        public decimal TotalAmount { get; set; } // Montant total de la commande
        [Required]
        public OrderStatus? Status { get; set; } 

        public DateTime? ShippingDate { get; set; } 
        public PaymentStatus? PaymentStatus { get; set; } 
        // Navigation properties
        public Customer Customer { get; set; }
        // List of order items
        public List<SalesOrderItem> Items { get; set; } = new List<SalesOrderItem>();
    }
}
