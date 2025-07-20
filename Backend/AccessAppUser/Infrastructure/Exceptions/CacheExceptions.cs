using System;

namespace AccessAppUser.Infrastructure.Exceptions
{
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
}