using System;

namespace AccessAppUser.Infrastructure.DTOs.GesPass
{
    /// <summary>
    /// DTO para la creación de una solicitud de cambio de contraseña.
    /// </summary>
    public class GesPassCreateDTO
    {
        public Guid UserId { get; set; }
        public string ResetToken { get; set; } = string.Empty;
        public DateTime TokenExpiration { get; set; }
    }
}
