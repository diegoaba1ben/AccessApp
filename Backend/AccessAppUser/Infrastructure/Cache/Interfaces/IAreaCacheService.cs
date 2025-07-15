using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Infrastructure.Cache.Interfaces
{
    public interface IAreaCacheService
    {
        Task SetAreaAsync(string key, Area area, TimeSpan expiration);
        Task<Area?> GetAreaAsync(string key);
        Task RemoveAreaAsync(string key);
    }
}
