using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Infrastructure.Cache.Interfaces
{
    public interface IPermissionCacheService
    {
        /// <summary>
        /// Obtiene un permiso de la caché por us ID
        /// </summary>
        /// <param name="id"> ID del permiso </param>
        /// <returns> Permiso </returns>
        Task<Permission?> GetPermissionAsync(Guid id);

        /// <summary>
        /// Establece un permiso en la caché
        /// </summary>
        /// <param name="Pemission"> Permiso a almacenar en la caché </param>
        Task SetPermissionAsync(Permission permission);

        /// <summary>
        /// Elimina un permiso de la caché por ID
        /// </summary>
        /// <parama name="id"> ID del permiso a eliminar </param>
        Task RemovePermissionAsync(Guid id);

        /// <summary>
        /// Obtiene permisos de la caché por ID de rol
        /// </summary>
        /// <param name="roleId"> ID del rol </param>
        /// <returns> Lista de permisos si se encuentran en la caché, o nulo si no </returns>
        Task<IEnumerable<Permission>?> GetPermissionsByRoleIdAsync(Guid roleId);

        /// <summary>
        /// Establece permisos en la caché por ID de rol
        /// </summary>
        /// <param name="permissions">Lista de permisos a almacenar en la caché</param>
        Task SetPermissionsByRoleIdAsync(Guid roleId, IEnumerable<Permission> permissions);

        /// <summary>
        /// Elimina permisos de la caché por Id de rol
        /// </summary>
        /// <parama name="roleId"> ID del rol </param>
        Task RemovePermissionsByRoleIdAsync(Guid roleId);
    }
}