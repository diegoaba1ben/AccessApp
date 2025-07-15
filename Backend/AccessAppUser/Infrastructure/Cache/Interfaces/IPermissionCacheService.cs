using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Infrastructure.Cache.Interfaces
{
    /// <summary>
    /// Contrato especializado para la gestión en caché de entidades <see cref="Permission"/>,
    /// tanto individuales como agrupadas por rol.
    /// </summary>
    public interface IPermissionCacheService
    {
        /// <summary>
        /// Recupera un permiso individual desde la caché utilizando su identificador.
        /// </summary>
        /// <param name="id">Identificador único del permiso.</param>
        /// <returns>Instancia <see cref="Permission"/> si se encuentra en caché; de lo contrario, <c>null</c>.</returns>
        Task<Permission?> GetPermissionAsync(Guid id);

        /// <summary>
        /// Almacena un permiso en caché con clave derivada del identificador.
        /// </summary>
        /// <param name="permission">Entidad <see cref="Permission"/> a almacenar.</param>
        Task SetPermissionAsync(Permission permission);

        /// <summary>
        /// Elimina un permiso de la caché por su identificador.
        /// </summary>
        /// <param name="id">Identificador del permiso a eliminar.</param>
        Task RemovePermissionAsync(Guid id);

        /// <summary>
        /// Recupera desde la caché los permisos asociados a un rol.
        /// </summary>
        /// <param name="roleId">Identificador único del rol.</param>
        /// <returns>Colección de <see cref="Permission"/> si se encuentra; de lo contrario, <c>null</c>.</returns>
        Task<IEnumerable<Permission>?> GetPermissionsByRoleIdAsync(Guid roleId);

        /// <summary>
        /// Almacena en caché una colección de permisos vinculados a un rol específico.
        /// </summary>
        /// <param name="roleId">Identificador del rol.</param>
        /// <param name="permissions">Colección de permisos a almacenar.</param>
        Task SetPermissionsByRoleIdAsync(Guid roleId, IEnumerable<Permission> permissions);

        /// <summary>
        /// Elimina de la caché todos los permisos relacionados a un rol.
        /// </summary>
        /// <param name="roleId">Identificador único del rol.</param>
        Task RemovePermissionsByRoleIdAsync(Guid roleId);
    }
}