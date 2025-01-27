using System;
using System.Collections.Generic;

namespace AccessAppUser.Domain.Entities
{
    /// <summary>
    /// Representa un área funcional dentro del sistema.
    /// </summary>
    public class Area
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Relación con Role
        public List<Role> Roles { get; set; } = new List<Role>();

        // Relación con la tabla intermedia AreaProfile
        public List<AreaProfile> AreaProfiles { get; set; } = new List<AreaProfile>();
    }
}
