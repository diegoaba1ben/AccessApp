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
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        /// <summary>
        /// Obtiene un usuario por su dirección de correo electrónico.
        /// </summary>
        public async Task<User?> GetByEmailAsync(string email)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower().Trim());

            if (user == null)
                Console.WriteLine($"No se encontró usuario con email: {email}");

            return user;
        }

        /// <summary>
        /// Obtiene un usuario con su perfil asociado.
        /// </summary>
        public async Task<User?> GetUserWithProfileAsync(Guid userId)
        {
            return await _context.Users
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        /// <summary>
        /// Obtiene un usuario con sus roles asociados.
        /// </summary>
        public async Task<User?> GetUserWithRolesAsync(Guid userId)
        {
            return await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
