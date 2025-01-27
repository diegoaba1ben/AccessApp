using System;
using System.Collections.Generic;

namespace AccessAppUser.Domain.Entities
{
    /// <summary>
    /// Representa un rol en el sistema.
    /// </summary>
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Relación con Permission
        public List<Permission> Permissions { get; set; } = new List<Permission>();
    }
}
