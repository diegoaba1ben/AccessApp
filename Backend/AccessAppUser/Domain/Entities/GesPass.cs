using System;
using AccessAppUser.Infrastructure.Helpers;

namespace AccessAppUser.Domain.Entities
{
    /// <summary>
    /// Entidad para gestionar cambios de contraseñas de usuarios.
    /// </summary>
    public class GesPass
    {
        public Guid UserId { get; set; } = Guid.Empty; // Id del usuario asociado
        public User User { get; set; } = null!; // Relación de navegación hacia User

        public string ResetToken { get; set; } = string.Empty; // Token para resetear la contraseña
        public DateTime TokenExpiration { get; set; } // Fecha de expiración del token
        public bool IsCompleted { get; set; } // Indica si el cambio de contraseña fue completado
        public DateTime? CompletedAt { get; set; } // Fecha de completado para auditoría

        /// <summary>
        /// Asocia un usuario y deriva el UserId.
        /// </summary>
        /// <param name="user">Usuario asociado</param>
        public void SetUser(User user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
            UserId = user.Id; // Deriva el Id del usuario
        }
    }
}
