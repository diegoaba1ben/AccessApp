using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Infrastructure.Queries.Interfaces
{
    /// <summary>
    /// Interfaz para las consultas relacionadas con la gestión de contraseñas.
    /// </summary>
    public interface IGesPassQueries
    {
        /// <summary>
        /// Obtiene todas las solicitudes de cambio de contraseña.
        /// </summary>
        Task<IEnumerable<GesPass>> GetAllGesPassesAsync();

        /// <summary>
        /// Obtiene una solicitud por el ID del usuario.
        /// </summary>
        Task<GesPass?> GetGesPassByUserIdAsync(Guid userId);

        /// <summary>
        /// Obtiene una solicitud por el token de restablecimiento.
        /// </summary>
        Task<GesPass?> GetGesPassByTokenAsync(string token);

        /// <summary>
        /// Obtiene las solicitudes con tokens expirados.
        /// </summary>
        Task<IEnumerable<GesPass>> GetGesPassesWithExpiredTokensAsync();

        /// <summary>
        /// Obtiene todas las solicitudes de cambio de contraseña existentes.
        /// </summary>
        Task<IEnumerable<GesPass>> GetAllPasswordsRequestsAsync();

        /// <summary>
        /// Obtiene una solicitud específica por correo electrónico del usuario.
        /// </summary>
        Task<GesPass?> GetPasswordRequestByUserMailAsync(string email);
    }
}
