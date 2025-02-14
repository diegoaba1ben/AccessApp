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
    public class PermissionQueries : IPermissionQueries
    {
        private readonly AppDbContext _context;

        public PermissionQueries(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Obtiene todos los permisos registrados en el sistema.
        /// </summary>
        public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
        {
            return await _context.Permissions.ToListAsync();
        }

        /// <summary>
        /// Obtiene los permisos asociados a un rol específico.
        /// </summary>
        public async Task<IEnumerable<Permission>> GetPermissionsByRoleAsync(string roleName)
        {
            return await _context.Permissions
                .Where(p => p.Roles.Any(r => r.Name == roleName))
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene los roles asociados a un permiso específico.
        /// </summary>
        public async Task<IEnumerable<Role>> GetRolesWithPermissionAsync(string permissionName)
        {
            return await _context.Roles
                .Include(r => r.Permissions)
                .Where(r => r.Permissions.Any(p => p.Name == permissionName))
                .ToListAsync();
        }
    }
}
