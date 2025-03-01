using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace AccessAppUser.Infrastructure.Cache
{
    public class RedisCacheService
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            var db = _redis.GetDatabase();
            var jsonData = JsonSerializer.Serialize(value);
            await db.StringSetAsync(key, jsonData, expiration);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var db = _redis.GetDatabase();
            var jsonData = await db.StringGetAsync(key);
            // Validación si es null o vacío
            if (jsonData.IsNullOrEmpty)
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(jsonData!);
        }

        public async Task RemoveAsync(string key)
        {
            var db = _redis.GetDatabase();
            await db.KeyDeleteAsync(key);
        }
    }
}
