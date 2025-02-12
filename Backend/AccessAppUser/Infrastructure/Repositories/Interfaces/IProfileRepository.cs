using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Infrastructure.Repositories.Interfaces
{
    public interface IProfileRepository : IBaseRepository<Profile>
    {
        /// <summary>
        /// Obtiene un perfil con su usuario asociado.
        /// </summary>
        Task<Profile?> GetProfileWithUserAsync(Guid profileId);

        /// <summary>
        /// Obtiene un perfil con su rol asociado.
        /// </summary>
        Task<Profile?> GetProfileWithRoleAsync(Guid profileId);

        /// <summary>
        /// Obtiene un perfil con sus áreas asociadas.
        /// </summary>
        Task<Profile?> GetProfileWithAreasAsync(Guid profileId);
    }
}
