using System;
using System.Collections.Generic;

namespace AccessAppUser.Infrastructure.DTOs.Profile
{
    /// <summary>
    /// DTO para la lectura de un perfil.
    /// </summary>
    public class ProfileReadDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public List<string> AssociatedAreas { get; set; } = new();
    }
}
