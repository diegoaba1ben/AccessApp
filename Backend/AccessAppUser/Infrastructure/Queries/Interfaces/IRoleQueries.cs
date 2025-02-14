using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Infrastructure.Queries.Interfaces
{
    /// <summary>
    /// Interfaz para las consultas relacionadas con la entidad Role
    /// </summary>
    public interface IRoleQueries
    {
        /// <summary>
        /// Obtiene todos los roles con sus respectivos permisos asociados
        /// </summary>
        
        Task<IEnumerable<Role>> GetRolesWithPermissionsAsync();

        /// <summary>
        /// Obtiene los roles asociados  a un área específica
        /// </summary>
        Task<IEnumerable<Role>> GetRolesByAreaAsync(string Name);

        /// <summary>
        /// Obtiene los roles que posee un permisos específico
        /// </summary>
        Task<IEnumerable<Role>> GetRolesWithSpecificPermissionAsync(string Name);
    }
}