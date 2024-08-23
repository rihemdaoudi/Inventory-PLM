using Inventory_PLM.Enums;
using System.ComponentModel.DataAnnotations;

namespace Inventory_PLM.Models.ViewModels
{
    public class SalesOrderViewModel
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(100)]
        public string Designation { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime SaleDate { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [Required]
        [StringLength(100)]
        public string StoreLocation { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus? Status { get; set; }

        public PaymentStatus? PaymentStatus { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ShippingDate { get; set; }

        // Liste des articles associés à la commande
        public List<SalesOrderItemViewModel> Items { get; set; } = new List<SalesOrderItemViewModel>();

        // Information sur le client (peut être utilisé dans la vue si nécessaire)
        public Customer Customer { get; set; }
    }


}

