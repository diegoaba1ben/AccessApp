using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Infrastructure.Repositories.Interfaces
{
    public interface IGesPassRepository : IBaseRepository<GesPass>
    {
        /// <summary>
        /// Obtiene la gestión de cambio de contraseña de un usuario por su ID.
        /// </summary>
        Task<GesPass?> GetByUserIdAsync(Guid userId);

        /// <summary>
        /// Obtiene la gestión de cambio de contraseña por el token de restablecimiento.
        /// </summary>
        Task<GesPass?> GetByResetTokenAsync(string token);
    }
}
