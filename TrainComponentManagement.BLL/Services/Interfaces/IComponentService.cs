using TrainComponentManagement.BLL.DTOs;

namespace TrainComponentManagement.BLL.Services.Interfaces
{
    public interface IComponentService
    {
        Task<ComponentDto> GetAsync(int id);
        Task<IEnumerable<ComponentDto>> GetAllAsync();
        Task<ComponentDto> CreateAsync(CreateOrUpdateComponentDto dto, string idempotencyKey);
        Task UpdateAsync(int id, CreateOrUpdateComponentDto dto);
        Task DeleteAsync(int id);
        Task BulkInsertAsync(IEnumerable<CreateOrUpdateComponentDto> dtos, string idempotencyKey);
        Task BulkDeleteAsync(IEnumerable<int> ids, string idempotencyKey);
    }
}
