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
    public class UserQueries : IUserQueries
    {
        private readonly AppDbContext _context;

        public UserQueries(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Obtiene una lista de todos los usuarios registrados.
        /// </summary>
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Profile)
                .Include(u => u.Roles)
                .Include(u => u.GesPass)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene los detalles completos de un usuario a partir de su correo electrónico.
        /// </summary>
        public async Task<User?> GetUserDetailsByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Profile)
                .Include(u => u.Roles)
                .Include(u => u.GesPass)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Obtiene una lista de usuarios asociados a un rol específico.
        /// </summary>
        public async Task<IEnumerable<User>> GetUsersByRoleAsync(string roleName)
        {
            return await _context.Users
                .Include(u => u.Roles)
                .Where(u => u.Roles.Any(r => r.Name == roleName))
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene una lista de usuarios según su estado (activo/inactivo).
        /// </summary>
        public async Task<IEnumerable<User>> GetUsersByStatusAsync(bool isActive)
        {
            return await _context.Users
                .Include(u => u.Profile)
                .Where(u => u.IsActive == isActive)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene un usuario junto con su gestión de cambio de contraseña.
        /// </summary>
        public async Task<User?> GetUserWithGesPassAsync(string email)
        {
            return await _context.Users
                .Include(u => u.GesPass)
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}