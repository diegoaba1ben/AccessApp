using System;

namespace AccessAppUser.Domain.Entities
{
    /// <summary>
    /// Clase intermedia que representa la relación muchos a muchos entre Role y Permission.
    /// </summary>
    public class RolePermission
    {
        public Guid RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public Guid PermissionId { get; set; }
        public Permission Permission { get; set; } = null!;

        // Atributos adicionales para auditoría
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}