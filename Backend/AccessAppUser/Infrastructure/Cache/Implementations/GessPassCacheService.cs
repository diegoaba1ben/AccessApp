using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Cache.Interfaces;
using AccessAppUser.Infrastructure.Helpers;

namespace AccessAppUser.Infrastructure.Cache.Implementations
{
    /// <summary>
    /// Servicio especializado para la gestión segura de entidades <see cref="GesPass"/> en caché.
    /// Aplica TTL corto y elimina proactivamente entradas finalizadas.
    /// </summary>
    public class GesPassCacheService : IGesPassCacheService
    {
        private readonly IBaseCacheService<GesPass> _cache;
        private readonly ILogger<GesPassCacheService> _logger;
        private static readonly TimeSpan DefaultTtl = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="GesPassCacheService"/>.
        /// </summary>
        /// <param name="cache">Servicio base para almacenar <see cref="GesPass"/>.</param>
        /// <param name="logger">Logger para trazabilidad y auditoría.</param>
        public GesPassCacheService(
            IBaseCacheService<GesPass> cache,
            ILogger<GesPassCacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task SetGesPassAsync(GesPass gesPass)
        {
            var key = CacheKeyFormatter.GetKey<GesPass>(gesPass.Id);

            if (gesPass.IsCompleted)
            {
                _logger.LogWarning("Intento de almacenar GesPass completado: {Key} — operación ignorada por política de seguridad.", key);
                return;
            }

            try
            {
                await _cache.SetAsync(key, gesPass, DefaultTtl);
                _logger.LogInformation("GesPass almacenado en caché con clave: {Key}", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al almacenar GesPass en caché: {Key}", key);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<GesPass?> GetGesPassAsync(Guid id)
        {
            var key = CacheKeyFormatter.GetKey<GesPass>(id);
            try
            {
                var gesPass = await _cache.GetAsync(key);
                if (gesPass == null)
                {
                    _logger.LogWarning("GesPass no encontrado en caché: {Key}", key);
                }
                return gesPass;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar GesPass en caché: {Key}", key);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task RemoveGesPassAsync(Guid id)
        {
            var key = CacheKeyFormatter.GetKey<GesPass>(id);
            try
            {
                await _cache.RemoveAsync(key);
                _logger.LogInformation("GesPass eliminado de la caché: {Key}", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar GesPass de la caché: {Key}", key);
                throw;
            }
        }
    }
}