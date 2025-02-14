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
    public class ProfileQueries : IProfileQueries
    {
        private readonly AppDbContext _context;

        public ProfileQueries(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Obtiene todos los perfiles registrados.
        /// </summary>
        public async Task<IEnumerable<Profile>> GetAllProfilesAsync()
        {
            return await _context.Profiles
                .Include(p => p.User)
                .Include(p => p.Role)
                .Include(p => p.AreaProfiles)
                    .ThenInclude(ap => ap.Area)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene los perfiles por estado (activos/inactivos).
        /// </summary>
        public async Task<IEnumerable<Profile>> GetProfilesByStatusAsync(bool isActive)
        {
            return await _context.Profiles
                .Include(p => p.User)
                .Include(p => p.Role)
                .Where(p => p.User.IsActive == isActive)  // Asumiendo que User tiene el campo IsActive
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene los detalles completos del perfil asociado a un correo electrónico.
        /// </summary>
        public async Task<Profile?> GetProfileDetailsByUserEmailAsync(string email)
        {
            return await _context.Profiles
                .Include(p => p.User)
                .Include(p => p.Role)
                .Include(p => p.AreaProfiles)
                    .ThenInclude(ap => ap.Area)
                .FirstOrDefaultAsync(p => p.User.Email == email);
        }

        /// <summary>
        /// Obtiene los perfiles asociados a un rol específico.
        /// </summary>
        public async Task<IEnumerable<Profile>> GetProfilesByRoleAsync(string roleName)
        {
            return await _context.Profiles
                .Include(p => p.User)
                .Include(p => p.Role)
                .Where(p => p.Role.Name == roleName)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene los perfiles asociados a un área específica.
        /// </summary>
        public async Task<IEnumerable<Profile>> GetProfilesByAreaAsync(string areaName)
        {
            return await _context.Profiles
                .Include(p => p.AreaProfiles)
                    .ThenInclude(ap => ap.Area)
                .Where(p => p.AreaProfiles.Any(ap => ap.Area.Name == areaName))
                .ToListAsync();
        }
    }
}
