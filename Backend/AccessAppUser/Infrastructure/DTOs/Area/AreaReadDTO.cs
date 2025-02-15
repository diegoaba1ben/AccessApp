using System;
using System.Collections.Generic;

namespace AccessAppUser.Infrastructure.DTOs.Area
{
    /// <summary>
    /// DTO para la lectura de un área.
    /// </summary>
    public class AreaReadDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> AssociatedRoles { get; set; } = new();
    }
}
