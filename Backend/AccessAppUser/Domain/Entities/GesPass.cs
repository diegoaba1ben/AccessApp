using System;
using System.ComponentModel.DataAnnotations;
using AccessAppUser.Infrastructure.Helpers;

namespace AccessAppUser.Domain.Entities
{
    /// <summary>
    /// Entidad para gestionar cambios de contrase�as de usuarios.
    /// </summary>
    public class GesPass
    {
        [Key] // Clave primaria
        public Guid Id { get; set; } = Guid.NewGuid(); // Identificador �nico
        public Guid UserId { get; set; } = Guid.Empty; // Id del usuario asociado
        public User User { get; set; } = null!; // Relaci�n de navegaci�n hacia User

        public string ResetToken { get; set; } = string.Empty; // Token para resetear la contrase�a
        public DateTime TokenExpiration { get; set; } // Fecha de expiraci�n del token
        public bool IsCompleted { get; set; } // Indica si el cambio de contrase�a fue completado
        public DateTime? CompletedAt { get; set; } // Fecha de completado para auditor�a

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
