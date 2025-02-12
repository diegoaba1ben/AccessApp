using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        /// <summary>
        /// Obtiene un usuario por su dirección de correo electrónico.
        /// </summary>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Obtiene un usuario junto con su perfil asociado.
        /// </summary>
        Task<User?> GetUserWithProfileAsync(Guid userId);

        /// <summary>
        /// Obtiene un usuario junto con sus roles asociados.
        /// </summary>
        Task<User?> GetUserWithRolesAsync(Guid userId);
    }
}
