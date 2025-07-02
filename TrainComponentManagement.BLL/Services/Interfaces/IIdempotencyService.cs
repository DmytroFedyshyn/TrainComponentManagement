namespace TrainComponentManagement.BLL.Services.Interfaces
{
    public interface IIdempotencyService
    {
        Task<bool> ExistsAsync(string key);
        Task<T> GetResultAsync<T>(string key);
        Task StoreResultAsync<T>(string key, T result);
    }
}
