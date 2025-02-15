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
    public class GesPassQueries
    {
        private readonly AppDbContext _context;

        public GesPassQueries(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<GesPass>> GetAllGesPassesAsync()
        {
            return await _context.GesPasses
                .Include(gp => gp.User)
                .ToListAsync();
        }

        public async Task<GesPass?> GetGesPassByUserIdAsync(Guid userId)
        {
            return await _context.GesPasses
                .Include(gp => gp.User)
                .FirstOrDefaultAsync(gp => gp.UserId == userId);
        }

        public async Task<GesPass?> GetGesPassByTokenAsync(string token)
        {
            return await _context.GesPasses
                .FirstOrDefaultAsync(gp => gp.ResetToken == token);
        }

        public async Task<IEnumerable<GesPass>> GetGesPassesWithExpiredTokensAsync()
        {
            return await _context.GesPasses
                .Where(gp => gp.TokenExpiration < DateTime.UtcNow && !gp.IsCompleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<GesPass>> GetAllPasswordsRequestsAsync()
        {
            return await _context.GesPasses
                .Include(gp => gp.User)
                .ToListAsync();
        }

        public async Task<GesPass?> GetPasswordRequestByUserMailAsync(string email)
        {
            return await _context.GesPasses
                .Include(gp => gp.User)
                .FirstOrDefaultAsync(gp => gp.User.Email == email);
        }
    }
}
