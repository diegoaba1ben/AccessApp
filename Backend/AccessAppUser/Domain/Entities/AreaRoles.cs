using System;

namespace AccessAppUser.Domain.Entities
{
    public class AreaRole
    {
        public Guid AreaId { get; set; }
        public Guid RoleId { get; set; }
        
        public Area Area { get; set; } = null!;
        public Role Role { get; set; } = null!;
    }
}