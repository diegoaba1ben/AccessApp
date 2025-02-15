using System;

namespace AccessAppUser.Infrastructure.DTOs.User
{
    /// <summary>
    /// DTO para la lectura de un usuario.
    /// </summary>
    public class UserReadDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}