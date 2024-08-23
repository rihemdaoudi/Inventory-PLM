using System.ComponentModel.DataAnnotations;

namespace Inventory_PLM.Models
{
    public class Category
    {
            [Key]
            public int ID { get; set; }

            [Required]
            [StringLength(100)]
            public string Name { get; set; }

            public string? Description { get; set; }
            public DateTime? CreatedDate { get; set; }
            public DateTime? ModifiedDate { get; set; }

            // Navigation property
            public ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();


    }
}
