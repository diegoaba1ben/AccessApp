using System;
using System.Threading.Tasks;

namespace AccessAppUser.Infrastructure.Cache.Interfaces
{
    /// <summary>
    /// Interfaz genérica para operaciones de caché.
    /// Define métodos estándar para almacenamiento, recuperación y eliminación de datos en Redis.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad que se almacenará en la caché.</typeparam>
    public interface IBaseCacheService<T> where T : class
    {
        /// <summary>
        /// Recupera una entidad desde la caché utilizando una clave específica.
        /// </summary>
        /// <param name="key">Clave asociada al valor almacenado.</param>
        /// <returns>Entidad si se encuentra; de lo contrario, <c>null</c>.</returns>
        Task<T?> GetAsync(string key);

        /// <summary>
        /// Almacena una entidad en la caché bajo la clave especificada y con expiración opcional.
        /// </summary>
        /// <param name="key">Clave bajo la cual se almacenará el objeto.</param>
        /// <param name="value">Entidad a serializar y guardar.</param>
        /// <param name="expiration">Duración opcional del valor en caché.</param>
        Task SetAsync(string key, T value, TimeSpan? expiration = null);

        /// <summary>
        /// Elimina una entidad de la caché utilizando su clave.
        /// </summary>
        /// <param name="key">Clave del objeto que se desea eliminar.</param>
        Task RemoveAsync(string key);
    }
}