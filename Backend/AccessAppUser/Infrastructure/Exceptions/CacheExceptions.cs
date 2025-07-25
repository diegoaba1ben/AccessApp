using System;

namespace AccessAppUser.Infrastructure.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando un valor en caché ha expirado antes de ser consultado.
    /// </summary>
    public class CacheExpiredException : AppException
    {
        /// <summary>
        /// Clave de caché asociada al valor expirado.
        /// </summary>
        public string CacheKey { get; }

        /// <summary>
        /// Tiempo de vida (TTL) máximo que tenía el valor en caché.
        /// </summary>
        public TimeSpan ExpiredTtl { get; }

        /// <summary>
        /// Constructor que permite especificar la clave y el tiempo expirado.
        /// </summary>
        /// <param name="key">Clave de caché utilizada.</param>
        /// <param name="ttl">Tiempo de vida máximo configurado.</param>
        public CacheExpiredException(string key, TimeSpan ttl)
            : base($"El valor cacheado para la clave '{key}' ha expirado (TTL: {ttl.TotalSeconds}s).")
        {
            CacheKey = key;
            ExpiredTtl = ttl;
        }
    }

    /// <summary>
    /// Excepción lanzada cuando no se encuentra un valor esperado en caché.
    /// </summary>
    public class CacheNotFoundException : AppException
    {
        /// <summary>
        /// Clave de caché que no pudo ser localizada.
        /// </summary>
        public string CacheKey { get; }

        /// <summary>
        /// Constructor que permite especificar la clave fallida.
        /// </summary>
        /// <param name="key">Clave que no se encontró en caché.</param>
        public CacheNotFoundException(string key)
            : base($"No se encontró ningún valor en caché para la clave '{key}'.")
        {
            CacheKey = key;
        }
    }

    /// <summary>
    /// Fábrica estática que centraliza la creación de excepciones cacheadas.
    /// </summary>
    public static class CacheExceptions
    {
        /// <summary>
        /// Genera excepción por valor expirado en caché.
        /// </summary>
        /// <param name="key">Clave cacheada</param>
        /// <param name="ttl">Tiempo de vida configurado</param>
        /// <returns>Instancia de CacheExpiredException</returns>
        public static CacheExpiredException Expired(string key, TimeSpan ttl) =>
            new CacheExpiredException(key, ttl);

        /// <summary>
        /// Genera excepción por valor inexistente en caché.
        /// </summary>
        /// <param name="key">Clave que no produjo resultado</param>
        /// <returns>Instancia de CacheNotFoundException</returns>
        public static CacheNotFoundException NotFound(string key) =>
            new CacheNotFoundException(key);
    }
}