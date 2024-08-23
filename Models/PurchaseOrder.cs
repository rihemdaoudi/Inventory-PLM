using System.ComponentModel.DataAnnotations;

namespace Inventory_PLM.Models
{
    public class PurchaseOrder
    {
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Code { get; set; }

        [Required]
        [StringLength(200)]
        public string Designation { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        public DateTime? CommissioningDate { get; set; }

        [Required]
        public int SupplierID { get; set; }

        [Required]
        [StringLength(100)]
        public string StoreLocation { get; set; }

        //[Required]
        //public int QuantityInStock { get; set; }

        [Required]
        public int AlertQuantity { get; set; }
        [Required]
        public decimal TotalAmount { get; set; } // Montant total de la commande

        [Required]
        public string Status { get; set; } // Statut de la commande (Pending, Approved, Received, etc.)


        // Navigation properties
        public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();
        public Supplier Supplier { get; set; }
    }

}
