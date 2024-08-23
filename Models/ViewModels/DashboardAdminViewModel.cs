namespace Inventory_PLM.Models.ViewModels
{
    public class DashboardAdminViewModel
    {
        public Dictionary<string, int> UserCountsByRole { get; set; }
        public int CategoriesCount { get; set; }
        public int SubCategoriesCount { get; set; }
        public int UnitsOfMeasureCount { get; set; }
       
    }
}
