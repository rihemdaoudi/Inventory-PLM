using Inventory_PLM.Models;

namespace Inventory_PLM.Interfaces.UnitOfMeasureInterface
{
    public interface IUnitOfMeasureService
    {
        Task<IEnumerable<UnitOfMeasure>> GetAllUnitsOfMeasureAsync();
        Task<UnitOfMeasure> GetUnitOfMeasureByIdAsync(int id);
        Task CreateUnitOfMeasureAsync(UnitOfMeasure unitOfMeasure);
        Task UpdateUnitOfMeasureAsync(UnitOfMeasure unitOfMeasure);
        Task DeleteUnitOfMeasureAsync(int id);
    }
}
