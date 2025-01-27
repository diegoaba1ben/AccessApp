using System;
using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Domain.Entities
{
    /// <summary>
    /// Tabla intermedia para relacionar áreas y perfiles.
    /// </summary>
    public class AreaProfile
    {
        public Guid AreaId { get; set; }
        public Area Area { get; set; } = null!;

        public Guid ProfileId { get; set; }
        public Profile Profile { get; set; } = null!;
    }
}

