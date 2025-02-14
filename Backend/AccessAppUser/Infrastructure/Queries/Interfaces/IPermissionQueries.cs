using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Infrastructure.Queries.Interfaces
{
    public interface IPermissionQueries
    {
        /// <summary>
        /// Obtiene todos los permisos registrados en el sistema.
        /// </summary>
        Task<IEnumerable<Permission>> GetAllPermissionsAsync();

        /// <summary>
        /// Obtiene los permisos asociados a un rol específico.
        /// </summary>
        Task<IEnumerable<Permission>> GetPermissionsByRoleAsync(string roleName);

        /// <summary>
        /// Obtiene los roles asociados a un permiso específico.
        /// </summary>
        Task<IEnumerable<Role>> GetRolesWithPermissionAsync(string permissionName);
    }
}
