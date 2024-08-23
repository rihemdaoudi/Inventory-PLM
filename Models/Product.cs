using System.ComponentModel.DataAnnotations;

namespace Inventory_PLM.Models
{
    public class Product
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Category ID is required.")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "SubCategory ID is required.")]
        public int SubCategoryID { get; set; }

        [Required(ErrorMessage = "Unit of Measure ID is required.")]
        public int UnitOfMeasureID { get; set; }

        [Required(ErrorMessage = "Code is required.")]
        [StringLength(100, ErrorMessage = "Code cannot exceed 100 characters.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters.")]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than or equal to 1.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Current Stock is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Current Stock must be 0 or greater.")]
        public int CurrentStock { get; set; }

        [Required(ErrorMessage = "Minimum Stock Level is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Minimum Stock Level must be 0 or greater.")]
        public int MinimumStockLevel { get; set; }

        [Required(ErrorMessage = "Status (Active/Inactive) is required.")]
        public bool IsActive { get; set; }

        public Category Category { get; set; }
        public SubCategory SubCategory { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();
    }
}
