using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Persistence;
using AccessAppUser.Infrastructure.Repositories.Base;
using AccessAppUser.Infrastructure.Repositories.Interfaces;

namespace AccessAppUser.Infrastructure.Repositories.Implementations
{
    public class AreaRepository : BaseRepository<Area>, IAreaRepository
    {
        public AreaRepository(AppDbContext context) : base(context) { }

        /// <summary>
        /// Obtiene todas las áreas con los roles asociados.
        /// </summary>
        public async Task<IEnumerable<Area>> GetAreasWithRolesAsync()
        {
            return await _context.Areas
                .Include(a => a.Roles)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene todas las áreas con los perfiles asociados.
        /// </summary>
        public async Task<IEnumerable<Area>> GetAreasWithProfilesAsync()
        {
            return await _context.Areas
                .Include(a => a.AreaProfiles)
                .ThenInclude(ap => ap.Profile)
                .ToListAsync();
        }
    }
}