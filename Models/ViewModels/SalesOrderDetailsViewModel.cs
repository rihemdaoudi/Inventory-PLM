using Inventory_PLM.Enums;

namespace Inventory_PLM.Models.ViewModels
{
    public class SalesOrderDetailsViewModel
    {
        public int ID { get; set; }

        public string Code { get; set; }

        public string Designation { get; set; }

        public DateTime SaleDate { get; set; }

        public int CustomerID { get; set; }

        public string StoreLocation { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; } // Valeur de Status avec une valeur par défaut

        public PaymentStatus PaymentStatus { get; set; } // Valeur de PaymentStatus avec une valeur par défaut

        public DateTime? ShippingDate { get; set; }

        public Customer Customer { get; set; }

        public List<SalesOrderItem> Items { get; set; } = new List<SalesOrderItem>();
    }
}
