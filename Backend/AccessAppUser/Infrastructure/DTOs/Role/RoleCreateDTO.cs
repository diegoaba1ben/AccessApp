using System;
using System.Collections.Generic;

namespace AccessAppUser.Infrastructure.DTOs.Role
{
    /// <summary>
    /// DTO para la creación de un rol.
    /// </summary>
    public class RoleCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Guid> PermissionIds { get; set; } = new();
    }
}
