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
    public class GesPassRepository : BaseRepository<GesPass>, IGesPassRepository
    {
        public GesPassRepository(AppDbContext context) : base(context) { }

        /// <summary>
        /// Obtiene la gestión de cambio de contraseña de un usuario por su ID.
        /// </summary>
        public async Task<GesPass?> GetByUserIdAsync(Guid userId)
        {
            return await _context.GesPasses
                .Include(gp => gp.User)
                .FirstOrDefaultAsync(gp => gp.UserId == userId);
        }

        /// <summary>
        /// Obtiene la gestión de cambio de contraseña por el token de restablecimiento.
        /// </summary>
        public async Task<GesPass?> GetByResetTokenAsync(string token)
        {
            return await _context.GesPasses
                .FirstOrDefaultAsync(gp => gp.ResetToken == token.Trim());
        }
    }
}