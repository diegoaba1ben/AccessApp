using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Persistence;
using AccessAppUser.Infrastructure.Queries.Interfaces;

namespace AccessAppUser.Infrastructure.Queries.Implementations
{
    public class AreaQueries : IAreaQueries
    {
        private readonly AppDbContext _context;

        public AreaQueries(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Area>> GetAllAreasAsync()
        {
            return await _context.Areas.ToListAsync();
        }

        public async Task<IEnumerable<Area>> GetAllAreasWithRolesAsync()
        {
            return await _context.Areas
                .Include(a => a.Roles)
                .ToListAsync();
        }

        public async Task<IEnumerable<Area>> GetAllAreasWithProfilesAsync()
        {
            return await _context.Areas
                .Include(a => a.AreaProfiles)
                .ThenInclude(ap => ap.Profile)
                .ToListAsync();
        }

        public async Task<IEnumerable<Area>> GetAreasByRoleNameAsync(string roleName)
        {
            return await _context.Areas
                .Where(a => a.Roles.Any(r => r.Name == roleName))
                .Include(a => a.Roles)
                .ToListAsync();
        }

        public async Task<IEnumerable<Area>> GetAreasByProfileIdAsync(Guid profileId)
        {
            return await _context.Areas
                .Where(a => a.AreaProfiles.Any(ap => ap.ProfileId == profileId))
                .Include(a => a.AreaProfiles)
                .ThenInclude(ap => ap.Profile)
                .ToListAsync();
        }
    }
}
