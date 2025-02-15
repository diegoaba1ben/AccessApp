using System;
using System.Collections.Generic;

namespace AccessAppUser.Infrastructure.DTOs.Profile
{
    /// <summary>
    /// DTO para la creación de un perfil.
    /// </summary>
    public class ProfileCreateDTO
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public List<Guid> AssociatedAreaIds { get; set; } = new();
    }
}
