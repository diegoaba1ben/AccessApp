using System;
using System.Collections.Generic;

namespace AccessAppUser.Domain.Entities
{
    /// <summary>
    /// Representa el perfil de un usuario dentro del sistema.
    /// </summary>
    public class Profile
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relación con User
        public User User { get; set; } = null!;

        // Relación con AreaProfile
        public List<AreaProfile> AreaProfiles { get; set; } = new List<AreaProfile>();

        // Relación con Role
        public Role Role { get; set; } = null!;
    }
}
