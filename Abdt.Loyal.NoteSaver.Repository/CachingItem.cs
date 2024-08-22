using Abdt.Loyal.NoteSaver.Repository.Abstractions;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Abdt.Loyal.NoteSaver.Repository
{
    public class CachingItem : ICache
    {
        private const string EmptySpaceErrorMessage = "Parameter {0} cannot be null or empty or white space";
        private readonly IDistributedCache _cache;

        public CachingItem(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> Get<T>(string id) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException(string.Format(EmptySpaceErrorMessage, nameof(id)));

            var key = GetKey(typeof(T).Name, id);
            var value = await _cache.GetStringAsync(key);

            return value is null ? null : JsonSerializer.Deserialize<T>(value);
        }

        public void Invalidate<T>(string id) where T : new()
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException(string.Format(EmptySpaceErrorMessage, nameof(id)));

            var key = GetKey(typeof(T).Name, id);
            _cache.Remove(key);
        }

        public async void Set<T>(string id, T item) where T : new()
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException(string.Format(EmptySpaceErrorMessage, nameof(id)));

            ArgumentNullException.ThrowIfNull(item);

            var key = GetKey(item.GetType().Name, id);
            await _cache.SetStringAsync(key, JsonSerializer.Serialize(item));
        }

        private string GetKey(string itemName, string id) => $"{itemName}:{id}";
    }
}
