using System;

namespace AccessAppUser.Infrastructure.DTOs.GesPass
{
    /// <summary>
    /// DTO para la lectura de una solicitud de cambio de contraseña.
    /// </summary>
    public class GesPassReadDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ResetToken { get; set; } = string.Empty;
        public DateTime TokenExpiration { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}