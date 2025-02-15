using System;

namespace AccessAppUser.Infrastructure.DTOs.Permission
{
    /// <summary>
    /// DTO para la creación de un permiso.
    /// </summary>
    public class PermissionCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
