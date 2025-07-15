using System;
using System.Threading.Tasks;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Cache.Interfaces;

namespace AccessAppUser.Infrastructure.Cache.Interfaces
{
    /// <summary>
    /// Contrato especializado para la gestión en caché de entidades <see cref="Profile"/>
    /// Define opereraciones para almacenamiento, recupeeración y eliminación basadas en claves externas
    /// </summary>
    public interface IProfileCacheService
    {
        /// <summary>
        /// Almacena una instancia de <see cref="Profile"/>   en caché su clave y duración personalizada.
        /// </summary>
        /// <param name="key"Clave única para la entrada en caché</param>
        /// <param name="profile">Entidad <see cref="Profile"/>a almacenar</param>
        /// <param name=expiration>Tiempo de vida en caché</param>
        Task SetProfileAsync(string key, Profile profile, TimeSpan expiration);

        /// <summary>
        /// Recupera una entidad <see cref="Profile"/ desde la caché según clave> 
        /// </summary>
        /// <param name="key">Clave asociada al objeto cacheado</param>
        /// <returns> Instancia <see cref="Profile"/>si se encuentra </returns>
        Task<Profile?> GetProfileAsync(string key);

        /// <summary>
        /// Elimina un perfil de la caché utilizando su identificador único
        /// </summary>
        /// <param name="key">Identificador único del perfil</param>
        Task RemoveProfileAsync(string key);
    }
}
