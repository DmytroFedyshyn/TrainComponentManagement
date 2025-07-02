using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using TrainComponentManagement.DAL.Data;
using TrainComponentManagement.DAL.Models;
using TrainComponentManagement.DAL.Repositories.Interfaces;

namespace TrainComponentManagement.DAL.Repositories.Implementation
{
    public class ComponentRepository : IComponentRepository
    {
        private readonly TrainComponentContext _ctx;

        private static readonly Func<TrainComponentContext, string, Task<Component?>>
            _getByUniqueNumberAsync = EF.CompileAsyncQuery(
                (TrainComponentContext ctx, string uniq) =>
                    ctx.Components.FirstOrDefault(c => c.UniqueNumber == uniq)
            );

        public ComponentRepository(TrainComponentContext ctx)
        {
            _ctx = ctx;
        }

        public Task<Component?> GetByIdAsync(int id) =>
            _ctx.Components.FindAsync(id).AsTask();

        public Task<Component?> GetByUniqueNumberAsync(string uniqueNumber) =>
            _getByUniqueNumberAsync(_ctx, uniqueNumber);

        public async Task<IEnumerable<Component>> GetAllAsync() =>
            await _ctx.Components
                      .AsNoTracking()
                      .ToListAsync();

        public async Task AddAsync(Component entity) =>
            await _ctx.Components.AddAsync(entity);

        public async Task AddRangeAsync(IEnumerable<Component> entities) =>
            await _ctx.Components.AddRangeAsync(entities);

        public async Task BulkInsertAsync(IEnumerable<Component> entities) =>
            await _ctx.BulkInsertAsync((IList<Component>)entities);

        public void Update(Component entity) =>
            _ctx.Components.Update(entity);

        public void Remove(Component entity) =>
            _ctx.Components.Remove(entity);

        public void RemoveRange(IEnumerable<Component> entities) =>
            _ctx.Components.RemoveRange(entities);

        public async Task BulkDeleteAsync(IEnumerable<Component> entities) =>
            await _ctx.BulkDeleteAsync((IList<Component>)entities);

        public async Task<bool> SaveChangesAsync() =>
            await _ctx.SaveChangesAsync() > 0;
    }
}
