using System.ComponentModel.DataAnnotations;

namespace Inventory_PLM.Models.ViewModels
{
    public class SalesOrderItemViewModel
    {
        public IEnumerable<SalesOrderItem> SalesOrderItems { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<SalesOrder> SalesOrders { get; set; }
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(100)]
        public string Designation { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal UnitPriceExclTax { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal UnitPriceInclTax { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal VAT { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public int RequestedQuantity { get; set; }

        [Range(0, double.MaxValue)]
        public int DeliveredQuantity { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }
    }
}

