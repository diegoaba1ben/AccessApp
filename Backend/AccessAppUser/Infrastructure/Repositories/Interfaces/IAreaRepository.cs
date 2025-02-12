using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Infrastructure.Repositories.Interfaces
{
    public interface IAreaRepository : IBaseRepository<Area>
    {
        /// <summary>
        /// Obtiene todas las áreas con los roles asociados.
        /// </summary>
        Task<IEnumerable<Area>> GetAreasWithRolesAsync();

        /// <summary>
        /// Obtiene todas las áreas con los perfiles asociados.
        /// </summary>
        Task<IEnumerable<Area>> GetAreasWithProfilesAsync();
    }
}
