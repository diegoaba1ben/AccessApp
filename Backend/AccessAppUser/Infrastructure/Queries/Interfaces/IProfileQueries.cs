using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Infrastructure.Queries.Interfaces
{
    public interface IProfileQueries
    {
        /// <summary>
        /// Obtiene todos los perfiles registrados.
        /// </summary>
        Task<IEnumerable<Profile>> GetAllProfilesAsync();

        /// <summary>
        /// Obtiene los perfiles por estado (activos/inactivos).
        /// </summary>
        Task<IEnumerable<Profile>> GetProfilesByStatusAsync(bool isActive);

        /// <summary>
        /// Obtiene los detalles completos del perfil asociado a un correo electrónico.
        /// </summary>
        Task<Profile?> GetProfileDetailsByUserEmailAsync(string email);

        /// <summary>
        /// Obtiene los perfiles asociados a un rol específico.
        /// </summary>
        Task<IEnumerable<Profile>> GetProfilesByRoleAsync(string roleName);

        /// <summary>
        /// Obtiene los perfiles asociados a un área específica.
        /// </summary>
        Task<IEnumerable<Profile>> GetProfilesByAreaAsync(string areaName);
    }
}
