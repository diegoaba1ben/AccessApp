using System;
using System.Threading.Tasks;
using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Infrastructure.Cache.Interfaces
{
    /// <summary>
    /// Contrato especializado para la gestión en caché de entidades <see cref="Role"/>.
    /// Hereda la funcionalidad genérica desde <see cref="IBaseCacheService{Role}"/>.
    /// </summary>
    public interface IRoleCacheService
    {
        /// <summary>
        /// Almacena una instancia de <see cref="Role"/> en caché con una clave personalizada y duración definida
        /// </summary>
        /// <param name="key">Identificador únicio para la entrada en caché </param>
        /// <param name="role">Entidad <see cref="Role"/> a almacenar  </param>
        /// <param name="expiration">Tiempo de vida en la caché (TTL) </param>
        Task SetRoleAsync(string key, Role role, TimeSpan expiration);

        /// <summary>
        /// Recupera una entidad <see cref="Role"/> desde la caché utilizando la clave proporcionada
        /// </summary>
        /// <param name="key"> Clave asociada al objeto almacenado </param>
        /// <returns> La entidad <see cref="Role"/> si se encuentra, de lo contrario null </returns>
        Task<Role?> GetRoleAsync(string key);

        /// <summary>
        /// Elimina una entrada en caché vinculada a la clave especificada.
        /// </summary>
        /// <param name="key"> Clave única del valor que se desea eliminar>
        Task RemoveRoleAsync(string key);
    }
}