using System.ComponentModel.DataAnnotations;

namespace Inventory_PLM.Models
{
    public class Inventory
    {
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Code { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(200)]
        public string Designation { get; set; }

        [Required]
        public int CategoryID { get; set; }

        [Required]
        [StringLength(100)]
        public string Location { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        [StringLength(500)]
        public string Observation { get; set; }

        // Navigation property
        public Category Category { get; set; }
    }
}
