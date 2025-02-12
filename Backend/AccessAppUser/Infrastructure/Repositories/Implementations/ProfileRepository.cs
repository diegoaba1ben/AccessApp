using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Persistence;
using AccessAppUser.Infrastructure.Repositories.Base;
using AccessAppUser.Infrastructure.Repositories.Interfaces;

namespace AccessAppUser.Infrastructure.Repositories.Implementations
{
    public class ProfileRepository : BaseRepository<Profile>, IProfileRepository
    {
        public ProfileRepository(AppDbContext context) : base(context) { }

        /// <summary>
        /// Obtiene un perfil con su usuario asociado.
        /// </summary>
        public async Task<Profile?> GetProfileWithUserAsync(Guid profileId)
        {
            return await _context.Profiles
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == profileId);
        }

        /// <summary>
        /// Obtiene un perfil con su rol asociado.
        /// </summary>
        public async Task<Profile?> GetProfileWithRoleAsync(Guid profileId)
        {
            return await _context.Profiles
                .Include(p => p.Role)
                .FirstOrDefaultAsync(p => p.Id == profileId);
        }

        /// <summary>
        /// Obtiene un perfil con sus áreas asociadas.
        /// </summary>
        public async Task<Profile?> GetProfileWithAreasAsync(Guid profileId)
        {
            return await _context.Profiles
                .Include(p => p.AreaProfiles)
                .ThenInclude(ap => ap.Area)
                .FirstOrDefaultAsync(p => p.Id == profileId);
        }
    }
}
