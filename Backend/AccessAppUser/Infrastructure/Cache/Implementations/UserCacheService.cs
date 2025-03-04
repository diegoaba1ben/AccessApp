using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Cache.Interfaces;

namespace AccessAppUser.Infrastructure.Cache.Implementations
{
    public class UserCacheService : IUserCacheService
    {
        private readonly IDatabase _cache;

        public UserCacheService(IConnectionMultiplexer redis)
        {
            _cache = redis.GetDatabase();
        }

        public async Task SetUserAsync(string key, User user, TimeSpan expiration)
        {
            var jsonData = JsonSerializer.Serialize(user);
            await _cache.StringSetAsync(key, jsonData, expiration);
        }

        public async Task<User?> GetUserAsync(string key)
        {
            var jsonData = await _cache.StringGetAsync(key);
            return jsonData.HasValue ? JsonSerializer.Deserialize<User>(jsonData!) : null;
        }

        public async Task RemoveUserAsync(string key)
        {
            await _cache.KeyDeleteAsync(key);
        }
    }
}

