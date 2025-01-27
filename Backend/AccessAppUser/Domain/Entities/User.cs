using System;

namespace AccessAppUser.Domain.Entities
{
    /// <summary>
    /// Representa un usuario del sistema.
    /// </summary>
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Relación con UserProfile
        public Profile? Profile { get; set; }
    }
}
