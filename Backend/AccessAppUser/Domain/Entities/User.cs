using System;
using AccessAppUser.Infrastructure.Helpers;


namespace AccessAppUser.Domain.Entities
{
    /// <summary>
    /// Representa un usuario del sistema.
    /// </summary>
    public class User
    {
        public Guid Id { get; set; } = GuidProvider.NewGuid(); // Id único del usuario
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public GesPass? GesPass { get; set; } // Relación con GesPass

        // Relación con UserProfile
        public Profile? Profile { get; set; }
    }
}
