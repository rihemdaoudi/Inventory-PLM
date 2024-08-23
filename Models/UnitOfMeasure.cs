using System.ComponentModel.DataAnnotations;

namespace Inventory_PLM.Models
{
    public class UnitOfMeasure
    {
        public int ID { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
