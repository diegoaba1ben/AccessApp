using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using AccessAppUser.Infrastructure.Cache.Interfaces;

namespace AccessAppUser.Infrastructure.Cache.Implementations
{
    /// <summary>
    /// Implementación genérica del servicio de caché para almacenamiento, recuperación y eliminación de entidades utilizando Redis.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad que será manejada en la caché.</typeparam>
    public class BaseCacheservice<T> : IBaseCacheService<T> where T : class
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _database;
        private readonly ILogger<BaseCacheservice<T>> _logger;
        private readonly TimeSpan _defaultExpiration = TimeSpan.FromMinutes(30);

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="BaseCacheservice{T}"/>.
        /// </summary>
        /// <param name="redis">Cliente de conexión a Redis.</param>
        /// <param name="logger">Instancia de logger para trazabilidad.</param>
        public BaseCacheservice(
            IConnectionMultiplexer redis,
            ILogger<BaseCacheservice<T>> logger)
        {
            _redis = redis;
            _database = redis.GetDatabase();
            _logger = logger;
        }

        /// <summary>
        /// Recupera una entidad desde la caché utilizando la clave especificada.
        /// </summary>
        /// <param name="key">Clave del objeto en caché.</param>
        /// <returns>Entidad deserializada si se encuentra; en caso contrario, <c>null</c>.</returns>
        public async Task<T?> GetAsync(string key)
        {
            try
            {
                var redisValue = await _database.StringGetAsync(key);
                if (redisValue.IsNullOrEmpty)
                {
                    _logger.LogWarning("No se encontró el valor en caché con la clave: {Key}", key);
                    return null;
                }

                return JsonSerializer.Deserialize<T>(redisValue);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error al deserializar el valor con la clave: {Key}", key);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción inesperada al obtener la clave: {Key} desde caché.", key);
                throw;
            }
        }

        /// <summary>
        /// Almacena una entidad en la caché con la clave indicada y una expiración opcional.
        /// </summary>
        /// <param name="key">Clave bajo la cual se guardará la entidad.</param>
        /// <param name="value">Entidad a almacenar.</param>
        /// <param name="expiration">Tiempo de vida en caché. Si no se especifica, se aplica un valor por defecto.</param>
        public async Task SetAsync(string key, T value, TimeSpan? expiration = null)
        {
            try
            {
                var serializedValue = JsonSerializer.Serialize(value);
                var ttl = expiration ?? _defaultExpiration;

                await _database.StringSetAsync(key, serializedValue, ttl);
                _logger.LogInformation("Valor almacenado en caché con la clave: {Key}", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al almacenar valor en caché con la clave: {Key}", key);
                throw;
            }
        }

        /// <summary>
        /// Elimina una entidad previamente almacenada en la caché utilizando la clave indicada.
        /// </summary>
        /// <param name="key">Clave del objeto a eliminar.</param>
        public async Task RemoveAsync(string key)
        {
            try
            {
                await _database.KeyDeleteAsync(key);
                _logger.LogInformation("Valor eliminado de la caché con la clave: {Key}", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar valor de la caché con la clave: {Key}", key);
                throw;
            }
        }
    }
}