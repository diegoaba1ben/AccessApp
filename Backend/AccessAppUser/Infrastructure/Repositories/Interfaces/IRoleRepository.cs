using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Repositories.Interfaces;

namespace AccessAppuser.Infrastruture.Repositories.Interfaces
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        /// <summary>
        /// Obtiene todos los roles junto con sus permisos asociados    
        /// </summary>
        /// <returns>Lista de roles con sus permisos asociados</returns>
        Task<IEnumerable<Role>> GetRolesWithPermissionsAsync();

        /// <summary>
        /// Obtiene los roles junto con sus áreas asociadas
        /// </summary>
        /// <returns>Lista de roles con sus áreas asociadas</returns>
        Task<IEnumerable<Role>> GetRolesWithAreasAsync();
    }
}