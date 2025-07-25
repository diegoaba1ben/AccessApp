using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Infrastructure.Repositories.Interfaces
{
    public interface IPermissionRepository : IBaseRepository<Permission>
    {

        /// <summary>
        /// Permite obtener permisos por  identificación de role
        /// </summary>
        Task<IEnumerable<Permission>> GetPermissionByRoleIdAsync(Guid roleId);

        /// <summary>
        /// Obtiene los permisos asociados a un rol usándo su nombre
        /// </summary>
        Task<IEnumerable<Permission>> GetPermissionByRoleNameAsync(string roleName);
    }
}