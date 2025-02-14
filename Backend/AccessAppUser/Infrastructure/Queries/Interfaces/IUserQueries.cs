using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Infrastructure.Queries.Interfaces
{
    public interface IUserQueries
    {
        /// <summary>
        /// Obtiene una lista de todos los usuarios registrados.
        /// </summary>
        Task<IEnumerable<User>> GetAllUsersAsync();

        /// <summary>
        /// Obtiene los detalles completos de un usuario a partir de su correo electrónico.
        /// </summary>
        Task<User?> GetUserDetailsByEmailAsync(string email);

        /// <summary>
        /// Obtiene una lista de usuarios asociados a un rol específico.
        /// </summary>
        Task<IEnumerable<User>> GetUsersByRoleAsync(string roleName);

        /// <summary>
        /// Obtiene una lista de usuarios según su estado (activo/inactivo).
        /// </summary>
        Task<IEnumerable<User>> GetUsersByStatusAsync(bool isActive);

        /// <summary>
        /// Obtiene un usuario junto con su gestión de cambio de contraseña.
        /// </summary>
        Task<User?> GetUserWithGesPassAsync(string email);
    }
}