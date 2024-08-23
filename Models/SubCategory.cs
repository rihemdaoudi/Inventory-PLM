using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory_PLM.Models
{
    public class SubCategory
    {
            [Key]
            public int ID { get; set; }
            [Required]
            [StringLength(100)]
            public string Name { get; set; }
            public string? Description { get; set; }

            [ForeignKey("Category")]
            public int CategoryID { get; set; }
            // Navigation property
            public Category Category { get; set; }
    }
}
