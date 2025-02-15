using System;
using System.Collections.Generic;

namespace AccessAppUser.Infrastructure.DTOs.Role
{
    /// <summary>
    /// DTO para la lectura de un rol.
    /// </summary>
    public class RoleReadDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Permissions { get; set; } = new();
    }
}
