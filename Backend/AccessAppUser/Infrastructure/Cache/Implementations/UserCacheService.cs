using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Cache.Interfaces;
using Microsoft.Extensions.Logging;

namespace AccessAppUser.Infrastructure.Cache.Implementations
{
    /// <summary>
    /// Servicio especializado para la gestión en caché de entidades.<see cref="User"/> usando Redis
    /// </summary>
    
    public class UserCacheService : IUserCacheService
    {
        private readonly IDatabase _cache;
        private readonly ILogger<UserCacheService> _logger;
        private readonly TimeSpan _defaultExpiration = TimeSpan.FromMinutes(30);

        /// <summary>
        /// Inicializa una nueva instancia del <see cref="UserCacheService"/>
        /// </summary>
        /// param name="redis">Multiplexor de conexión a Redis</param>
        /// param name="logger>Instancia de logger para trazas y manejo de errores</param>"

        public UserCacheService(IConnectionMultiplexer redis, ILogger<UserCacheService> logger)
        {
            _cache = redis.GetDatabase();
            _logger = logger;
        }
        /// <summary>
        /// Guarda una instancia <see cref="User"/> en la caché con una clave específica y un tiempo de expiración.
        /// </summary>
        /// <param name="key">Clave única bajo la cual se almacenará el objeto.</param>
        /// <param name="user">Objeto <see cref="User"/> a almacenar y guardar.</param>
        /// <param name="expiration"Duración que el valor permanecerá en la caché.</param>

        public async Task SetUserAsync(string key, User user, TimeSpan expiration)
        {
            try
            {
                var jsonData = JsonSerializer.Serialize(user);
                await _cache.StringSetAsync(key, jsonData, expiration);
                _logger.LogInformation("Usuario con clave {Key} almacenado en caché con éxito.", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al almacenar el usuario con clave {Key} en la caché.", key);
                throw;
              }
        }
        /// <summary>
        /// Recupera un objeto <see cref="User"/> de la caché usando una clave específica.
        /// </summary>
        /// <param name="key">Clave única bajo la cual se almacenó el objeto.</param>
        /// <returns>Objeto <see cref="User"/> si se encuentra en la caché, de lo contrario, null.</returns>    

        public async Task<User?> GetUserAsync(string key)
        {
            try
            {
                var jsonData = await _cache.StringGetAsync(key);
                if (jsonData.IsNullOrEmpty)
                {
                    _logger.LogWarning("Usuario con clave {key} no encontrado en la caché.", key);
                    return null;
                }
                return null;
            }
            catch (Exception ex)
            { 
                _logger.LogError(ex, "Error inesperada al recuperar el usuario", key);
                throw;
            }
        }
        /// <summary>
        /// Elimina una entidad <see cref="User"/> previamente almacenada en la caché 
        /// </summary>
        /// <param name="key">Clave Única del usuario que se desea eliminar</param>
        public async Task RemoveUserAsync(string key)
        {
            try
            {
                await _cache.KeyDeleteAsync(key);
                _logger.LogInformation("Usuario con clave {Key} eliminado de la caché.", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el usuario con clave {Key} de la caché.", key);
                throw;
             }
        }
    }
}

