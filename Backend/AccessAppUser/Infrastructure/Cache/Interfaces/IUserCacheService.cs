using System;
using System.Threading.Tasks;
using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Infrastructure.Cache.Interfaces
{
    /// <summary>
    /// Contrato especializado para la gestión en caché de entidades <see cref="User"/>.
    /// Define operaciones estándar para almacenamiento, recuperación y eliminación usando claves externas.
    /// </summary>
    public interface IUserCacheService
    {
        /// <summary>
        /// Almacena una instancia de <see cref="User"/> en caché con una clave personalizada y duración definida.
        /// </summary>
        /// <param name="key">Identificador único para la entrada en caché.</param>
        /// <param name="user">Entidad <see cref="User"/> a almacenar.</param>
        /// <param name="expiration">Tiempo de vida en la caché (TTL).</param>
        Task SetUserAsync(string key, User user, TimeSpan expiration);

        /// <summary>
        /// Recupera una entidad <see cref="User"/> desde la caché utilizando la clave proporcionada.
        /// </summary>
        /// <param name="key">Clave asociada al objeto almacenado.</param>
        /// <returns>La entidad <see cref="User"/> si se encuentra, de lo contrario <c>null</c>.</returns>
        Task<User?> GetUserAsync(string key);

        /// <summary>
        /// Elimina una entrada en caché vinculada a la clave especificada.
        /// </summary>
        /// <param name="key">Clave única del valor que se desea eliminar.</param>
        Task RemoveUserAsync(string key);
    }
}