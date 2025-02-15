using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Infrastructure.Queries.Interfaces
{
    /// <summary>
    /// Interfaz para las consultas relacionadas con las áreas.
    /// </summary>
    public interface IAreaQueries
    {
        /// <summary>
        /// Obtiene todas las áreas existentes
        /// </summary>
        Task<IEnumerable<Area>> GetAllAreasAsync();

        /// <summary>
        /// Obtiene todas las áreas con sus roles asociados.
        /// </summary>
        Task<IEnumerable<Area>> GetAllAreasWithRolesAsync();

        /// <summary>
        /// Obtiene todas las áreas con sus perfiles asociados.
        /// </summary>
        Task<IEnumerable<Area>> GetAllAreasWithProfilesAsync();

        /// <summary>
        /// Obtiene las áreas asociadas a un rol específico
        /// </summary>
        /// <Param name="roleName">Nombre del rol </param>
        Task<IEnumerable<Area>> GetAreasByRoleNameAsync(string roleName);

        /// <summary>
        /// Obtiene las áreas asociadas a un perfil específico
        /// </summary>
        /// <param name="profileId">Id del perfil</param>
        Task<IEnumerable<Area>> GetAreasByProfileIdAsync(Guid profileId);
    }
}