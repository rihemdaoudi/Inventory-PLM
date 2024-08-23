using Inventory_PLM.Enums;
using System.ComponentModel.DataAnnotations;

namespace Inventory_PLM.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Reference { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; } 

        [Required]
        public decimal VATRate { get; set; } 

        [Required]
        public decimal TotalAmount { get; set; } 
        [Required]
        public decimal AmountExclVAT { get; set; } 

        [Required]
        public decimal AmountInclVAT { get; set; } 

        [Required]
        public InvoiceStatus Status { get; set; } // Enum for status

        public string? Comments { get; set; }
    }

   
}
