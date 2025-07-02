using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrainComponentManagement.BLL.DTOs;
using TrainComponentManagement.BLL.Services.Interfaces;
using TrainComponentManagement.DAL.Data;
using TrainComponentManagement.DAL.Models;
using TrainComponentManagement.DAL.Repositories.Interfaces;

namespace TrainComponentManagement.BLL.Services.Implementation
{
    public class ComponentService : IComponentService
    {
        private readonly IComponentRepository _repo;
        private readonly IMapper _mapper;
        private readonly TrainComponentContext _context;
        private readonly IIdempotencyService _idem;

        public ComponentService(
            IComponentRepository repo,
            IMapper mapper,
            TrainComponentContext context,
            IIdempotencyService idempotencyService)
        {
            _repo = repo;
            _mapper = mapper;
            _context = context;
            _idem = idempotencyService;
        }

        public async Task<ComponentDto> CreateAsync(CreateOrUpdateComponentDto dto, string idempotencyKey)
        {
            if (await _idem.ExistsAsync(idempotencyKey))
                return await _idem.GetResultAsync<ComponentDto>(idempotencyKey);

            ComponentDto result = null!;

            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await _context.Database.BeginTransactionAsync();

                var entity = _mapper.Map<Component>(dto);
                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync();

                result = _mapper.Map<ComponentDto>(entity);

                await tx.CommitAsync();
            });

            await _idem.StoreResultAsync(idempotencyKey, result);
            return result;
        }

        public async Task UpdateAsync(int id, CreateOrUpdateComponentDto dto)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await _context.Database.BeginTransactionAsync();

                var entity = await _repo.GetByIdAsync(id)
                    ?? throw new KeyNotFoundException($"Component {id} not found.");

                _mapper.Map(dto, entity);
                _repo.Update(entity);
                await _repo.SaveChangesAsync();

                await tx.CommitAsync();
            });
        }

        public async Task DeleteAsync(int id)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await _context.Database.BeginTransactionAsync();

                var entity = await _repo.GetByIdAsync(id)
                    ?? throw new KeyNotFoundException($"Component {id} not found.");

                _repo.Remove(entity);
                await _repo.SaveChangesAsync();

                await tx.CommitAsync();
            });
        }

        public async Task<ComponentDto> GetAsync(int id)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                var entity = await _repo.GetByIdAsync(id)
                    ?? throw new KeyNotFoundException($"Component {id} not found.");
                return _mapper.Map<ComponentDto>(entity);
            });
        }

        public async Task<IEnumerable<ComponentDto>> GetAllAsync()
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                var list = await _repo.GetAllAsync();
                return _mapper.Map<IEnumerable<ComponentDto>>(list);
            });
        }

        public async Task BulkInsertAsync(IEnumerable<CreateOrUpdateComponentDto> dtos, string idempotencyKey)
        {
            if (await _idem.ExistsAsync(idempotencyKey))
                return;

            var entities = dtos.Select(dto => _mapper.Map<Component>(dto)).ToList();

            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await _context.Database.BeginTransactionAsync();
                await _repo.BulkInsertAsync(entities);
                await tx.CommitAsync();
            });

            await _idem.StoreResultAsync(idempotencyKey, entities.Count);
        }

        public async Task BulkDeleteAsync(IEnumerable<int> ids, string idempotencyKey)
        {
            if (await _idem.ExistsAsync(idempotencyKey))
                return;

            var allComponents = await _repo.GetAllAsync();
            var toDelete = allComponents
                .Where(c => ids.Contains(c.Id))
                .ToList();

            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await _context.Database.BeginTransactionAsync();
                await _repo.BulkDeleteAsync(toDelete);
                await tx.CommitAsync();
            });

            await _idem.StoreResultAsync(idempotencyKey, toDelete.Count);
        }
    }
}
