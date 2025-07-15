using System;
using System.Threading.Tasks;
using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Infrastructure.Cache.Interfaces
{
    /// <summary>
    /// Contrato especializado para el almacenamiento seguro y controlado de entidades <see cref="GesPass"/> en caché.
    /// Este servicio aplica TTL reducido y eliminación proactiva basada en estado de finalización.
    /// </summary>
    public interface IGesPassCacheService
    {
        /// <summary>
        /// Recupera una entidad <see cref="GesPass"/> desde la caché utilizando su identificador único.
        /// </summary>
        /// <param name="id">Identificador del objeto GesPass.</param>
        /// <returns>Instancia <see cref="GesPass"/> si se encuentra activa; de lo contrario, <c>null</c>.</returns>
        Task<GesPass?> GetGesPassAsync(Guid id);

        /// <summary>
        /// Almacena una instancia de <see cref="GesPass"/> en caché, aplicando TTL corto y validación de estado.
        /// </summary>
        /// <param name="gesPass">Entidad GesPass a almacenar.</param>
        Task SetGesPassAsync(GesPass gesPass);

        /// <summary>
        /// Elimina una entrada <see cref="GesPass"/> de la caché utilizando su identificador único.
        /// </summary>
        /// <param name="id">Identificador del objeto GesPass a eliminar.</param>
        Task RemoveGesPassAsync(Guid id);
    }
}