using Inventory_PLM.Data;
using Inventory_PLM.Interfaces.SubCategoryInterface;
using Inventory_PLM.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory_PLM.Services.SubCategoryService
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly ApplicationDbContext _context;

        public SubCategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SubCategory>> GetAllSubCategoriesAsync()
        {
            return await _context.SubCategories.ToListAsync();
        }

        public async Task<SubCategory> GetSubCategoryByIdAsync(int id)
        {
            return await _context.SubCategories.FindAsync(id);
        }

        public async Task CreateSubCategoryAsync(SubCategory subCategory)
        {
            _context.SubCategories.Add(subCategory);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSubCategoryAsync(SubCategory subCategory)
        {
            _context.Entry(subCategory).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSubCategoryAsync(int id)
        {
            var subCategory = await _context.SubCategories.FindAsync(id);
            if (subCategory != null)
            {
                _context.SubCategories.Remove(subCategory);
                await _context.SaveChangesAsync();
            }
        }
    }
}
