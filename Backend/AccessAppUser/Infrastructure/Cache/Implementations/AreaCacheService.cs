using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Cache.Interfaces;

namespace AccessAppUser.Infrastructure.Cache.Implementations
{
    /// <summary>
    /// Servicio de caché específico para entidad <see cref="Area"/> utilizando una implementación base genérica.
    /// </summary>
    public class AreaCacheService : IAreaCacheService
    {
        private readonly IBaseCacheService<Area> _cache;
        private readonly ILogger<AreaCacheService> _logger;

        /// <summary>
        /// Inicializa una nueva instancia del <see cref="AreaCacheService"/>.
        /// </summary>
        /// <param name="cache">Servicio de caché genérico para almacenar áreas.</param>
        /// <param name="logger">Instancia de logger para registrar eventos relacionados con la caché.</param>
        public AreaCacheService(
            IBaseCacheService<Area> cache,
            ILogger<AreaCacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        /// <summary>
        /// Almacena un objeto <see cref="Area"/> en la caché con una clave personalizada y un tiempo de expiración definido.
        /// </summary>
        /// <param name="key">Clave bajo la cual se almacena el objeto.</param>
        /// <param name="area">Instancia del área que se va a guardar.</param>
        /// <param name="expiration">Duración durante la cual el valor permanecerá en la caché.</param>
        public async Task SetAreaAsync(string key, Area area, TimeSpan expiration)
        {
            try
            {
                await _cache.SetAsync(key, area, expiration);
                _logger.LogInformation("Área almacenada en caché con la clave: {Key}", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al almacenar el área en caché con la clave: {Key}", key);
                throw;
            }
        }

        /// <summary>
        /// Recupera un objeto <see cref="Area"/> desde la caché utilizando la clave proporcionada.
        /// </summary>
        /// <param name="key">Clave de la entidad almacenada.</param>
        /// <returns>El área correspondiente si se encuentra; en caso contrario, <c>null</c>.</returns>
        public async Task<Area?> GetAreaAsync(string key)
        {
            try
            {
                var area = await _cache.GetAsync(key);
                if (area == null)
                {
                    _logger.LogInformation("No se encontró el área con la clave: {Key}", key);
                }
                return area;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar el área de la caché con la clave: {Key}", key);
                return null; // Retorna null si ocurre un error al intentar obtener el área
            }
        }

        /// <summary>
        /// Elimina un objeto <see cref="Area"/> previamente almacenado en la caché.
        /// </summary>
        /// <param name="key">Clave asociada al área que se desea eliminar.</param>
        public async Task RemoveAreaAsync(string key)
        {
            try
            {
                await _cache.RemoveAsync(key);
                _logger.LogInformation("Área eliminada de la caché con la clave: {Key}", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el área de la caché con la clave: {Key}", key);
                throw;
            }
        }
    }
}
