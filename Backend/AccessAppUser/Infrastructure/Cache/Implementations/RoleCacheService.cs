using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Cache.Interfaces;

namespace AccessApp.Infrastructure.Cache.Implementations
{
    /// <summary>
    /// Servicio concreto para operaciones de caché sobre entidades <see cref="Role"/>.
    /// Aplica el Patrón del método estructurado CME y delega funcionalidad en <see cref="IBaseCacheService{Role}"/>.
    /// </summary>
    public class RoleCacheService : IRoleCacheService
    {
        private readonly IBaseCacheService<Role> _cache;
        private readonly ILogger<RoleCacheService> _logger;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="RoleCacheService"/>.
        /// </summary>
        /// <param name="cache">Instancia de servicio base de caché para <see cref="Role"/>.</param>
        /// <param name="logger">Logger para trazabilidad de eventos y errores.</param>
        public RoleCacheService(
            IBaseCacheService<Role> cache,
            ILogger<RoleCacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task SetRoleAsync(string key, Role role, TimeSpan expiration)
        {
            try
            {
                await _cache.SetAsync(key, role, expiration);
                _logger.LogInformation("Role con clave {Key} almacenado correctamente en caché.", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al almacenar Role con clave {Key} en la caché.", key);
                throw;
            }
        }

        public async Task<Role?> GetRoleAsync(string key)
        {
            try
            {
                var role = await _cache.GetAsync(key);
                if (role == null)
                {
                    _logger.LogWarning("Role con clave {Key} no encontrado en la caché.", key);
                }
                return role;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar Role con clave {Key} de la caché.", key);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task RemoveRoleAsync(string key)
        {
            try
            {
                await _cache.RemoveAsync(key);
                _logger.LogInformation("Role con clave {Key} eliminado correctamente de la caché.", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar Role con clave {Key} de la caché.", key);
                throw;
            }
        }
    }
}