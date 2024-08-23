using Inventory_PLM.Data;
using Inventory_PLM.Interfaces.UnitOfMeasureInterface;
using Inventory_PLM.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory_PLM.Services.UnitOfMeasureService
{
    public class UnitOfMeasureService : IUnitOfMeasureService
    {
        private readonly ApplicationDbContext _context;

        public UnitOfMeasureService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UnitOfMeasure>> GetAllUnitsOfMeasureAsync()
        {
            return await _context.UnitOfMeasures.ToListAsync();
        }

        public async Task<UnitOfMeasure> GetUnitOfMeasureByIdAsync(int id)
        {
            return await _context.UnitOfMeasures.FindAsync(id);
        }

        public async Task CreateUnitOfMeasureAsync(UnitOfMeasure unitOfMeasure)
        {
            _context.UnitOfMeasures.Add(unitOfMeasure);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUnitOfMeasureAsync(UnitOfMeasure unitOfMeasure)
        {
            _context.Entry(unitOfMeasure).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUnitOfMeasureAsync(int id)
        {
            var unitOfMeasure = await _context.UnitOfMeasures.FindAsync(id);
            if (unitOfMeasure != null)
            {
                _context.UnitOfMeasures.Remove(unitOfMeasure);
                await _context.SaveChangesAsync();
            }
        }
    }
}
