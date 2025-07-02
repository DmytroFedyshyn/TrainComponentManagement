using TrainComponentManagement.DAL.Models;

namespace TrainComponentManagement.DAL.Repositories.Interfaces
{
    public interface IComponentRepository
    {
        Task<Component?> GetByIdAsync(int id);
        Task<Component?> GetByUniqueNumberAsync(string uniqueNumber);
        Task<IEnumerable<Component>> GetAllAsync();
        Task AddAsync(Component entity);
        Task AddRangeAsync(IEnumerable<Component> entities);
        Task BulkInsertAsync(IEnumerable<Component> entities);
        void Update(Component entity);
        void Remove(Component entity);
        void RemoveRange(IEnumerable<Component> entities);
        Task BulkDeleteAsync(IEnumerable<Component> entities);
        Task<bool> SaveChangesAsync();
    }
}
