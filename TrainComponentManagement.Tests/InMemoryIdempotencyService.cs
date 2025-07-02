using System.Collections.Concurrent;
using TrainComponentManagement.BLL.Services.Interfaces;

namespace TrainComponentManagement.Tests
{
    public class InMemoryIdempotencyService : IIdempotencyService
    {
        private readonly ConcurrentDictionary<string, object> _store
            = new();

        public Task<bool> ExistsAsync(string key) =>
            Task.FromResult(_store.ContainsKey(key));

        public Task<T> GetResultAsync<T>(string key) =>
            Task.FromResult((T)_store[key]!);

        public Task StoreResultAsync<T>(string key, T result)
        {
            _store[key] = result!;
            return Task.CompletedTask;
        }
    }
}
