using System;

namespace AccessAppUser.Infrastructure.DTOs.Permission
{
    /// <summary>
    /// DTO para la lectura de un permiso.
    /// </summary>
    public class PermissionReadDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
