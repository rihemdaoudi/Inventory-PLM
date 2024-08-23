using Inventory_PLM.Models;
using System.Threading.Tasks;

namespace Inventory_PLM.Interfaces.SubCategoryInterface
{
    public interface ISubCategoryService
    {
        Task<IEnumerable<SubCategory>> GetAllSubCategoriesAsync();
        Task<SubCategory> GetSubCategoryByIdAsync(int id);
        Task CreateSubCategoryAsync(SubCategory subCategory);
        Task UpdateSubCategoryAsync(SubCategory subCategory);
        Task DeleteSubCategoryAsync(int id);
    }
}
