using System;
using System.Threading.Tasks;
using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Infrastructure.Cache.Interfaces
{
    public interface IUserCacheService
    {
        Task SetUserAsync(string key, User User, TimeSpan expiration);
        Task<User?>GetUserAsync(string key);
        Task RemoveUserAsync(string key);
    }
}