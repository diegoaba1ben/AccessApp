using System;
using System.Collections.Generic;

namespace AccessAppUser.Infrastructure.DTOs.Area
{
    /// <summary>
    /// DTO para la creación de un área.
    /// </summary>
    public class AreaCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Guid> AssociatedRolesIds { get; set; } = new();
    }
}
