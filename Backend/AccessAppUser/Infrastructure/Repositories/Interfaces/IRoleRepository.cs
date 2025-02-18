using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Infrastructure.Repositories.Interfaces
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        /// <summary>
        /// Obtiene todos los roles junto con sus permisos asociados    
        /// </summary>
        Task<IEnumerable<Role>> GetRolesWithPermissionsAsync();

        /// <summary>
        /// Obtiene los roles junto con sus áreas asociadas
        /// </summary>
        Task<IEnumerable<Role>> GetRolesWithAreasAsync();
        
        /// <summary>
        /// Obtener permisos por ID de Roles
        /// </summary>
        Task<IEnumerable<Permission>> GetPermissionsByRoleIdAsync(Guid roleId);

        /// <summary>
        /// Asigna un permiso a un rol
        /// </summary>
        Task<bool> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId);

        /// <summary>
        /// Elimina un permiso de un rol
        /// </summary>
        Task<bool> RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId);

        /// <summary>
        /// Obtiene usuarios por identificación de roles
        /// </summary>
        Task<IEnumerable<User>> GetUsersByRoleIdAsync(Guid roleId);

        /// <summary>
        /// Obtiene áreas por identificación de roles
        /// </summary>
        Task<IEnumerable<Area>> GetAreasByRoleIdAsync(Guid roleId);
    }
}
