using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Cache.Interfaces;

namespace AccessAppUser.Infrastructure.Cache.Implementations
{
    /// <summary>
    /// Servicio especializado para la gestión en caché de entidades <see cref="Profile"/>.
    /// Delegado en la infraestructura genérica <see cref="IBaseCacheService{Profile}"/>.
    /// </summary>
    public class ProfileCacheService : IProfileCacheService
    {
        private readonly IBaseCacheService<Profile> _cache;
        private readonly ILogger<ProfileCacheService> _logger;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ProfileCacheService"/>.
        /// </summary>
        /// <param name="cache">Servicio base de caché para <see cref="Profile"/>.</param>
        /// <param name="logger">Instancia de logger para trazabilidad.</param>
        public ProfileCacheService(
            IBaseCacheService<Profile> cache,
            ILogger<ProfileCacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        /// <summary>
        /// Almacena una instancia de <see cref="Profile"/> en caché con una clave y duración definida.
        /// </summary>
        /// <param name="key">Clave única para la entrada en caché.</param>
        /// <param name="profile">Entidad <see cref="Profile"/> a almacenar.</param>
        /// <param name="expiration">Duración durante la cual el objeto permanecerá en caché.</param>
        public async Task SetProfileAsync(string key, Profile profile, TimeSpan expiration)
        {
            try
            {
                await _cache.SetAsync(key, profile, expiration);
                _logger.LogInformation("Profile almacenado en caché con clave: {Key}", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al almacenar el Profile con clave: {Key}", key);
                throw;
            }
        }

        /// <summary>
        /// Recupera una entidad <see cref="Profile"/> desde la caché utilizando una clave específica.
        /// </summary>
        /// <param name="key">Clave única utilizada en la caché.</param>
        /// <returns>Instancia <see cref="Profile"/> si se encuentra; de lo contrario, <c>null</c>.</returns>
        public async Task<Profile?> GetProfileAsync(string key)
        {
            try
            {
                var profile = await _cache.GetAsync(key);
                if (profile == null)
                {
                    _logger.LogWarning("Profile con clave {Key} no encontrado en caché.", key);
                }
                return profile;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar el Profile con clave: {Key}", key);
                throw;
            }
        }

        /// <summary>
        /// Elimina una entidad <see cref="Profile"/> de la caché según su clave.
        /// </summary>
        /// <param name="key">Clave única del objeto a eliminar.</param>
        public async Task RemoveProfileAsync(string key)
        {
            try
            {
                await _cache.RemoveAsync(key);
                _logger.LogInformation("Profile eliminado correctamente de la caché con clave: {Key}", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el Profile de la caché con clave: {Key}", key);
                throw;
            }
        }
    }
}