using System;

namespace AccessAppUser.Domain.Entities
{
    /// <summary>
    /// Representa un permiso dentro del sistema.
    /// </summary>
    public class Permission
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
