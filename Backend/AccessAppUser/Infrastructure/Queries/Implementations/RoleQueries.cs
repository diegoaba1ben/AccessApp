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
    public class RoleQueries : IRoleQueries
    {
        private readonly AppDbContext _context;
        
        public RoleQueries(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Obtiene todos los roles con sus permisos.
        /// </summary>
        public async Task<IEnumerable<Role>> GetRolesWithPermissionsAsync()
        {
            return await _context.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene los roles asociados a un área específica.
        /// </summary>
        public async Task<IEnumerable<Role>> GetRolesByAreaAsync(string name)
        {
            return await _context.Roles 
                .Where(r => r.Areas.Any(a => a.Name == name))
                .Include(r => r.Areas)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene los roles que poseen un permiso específico.
        /// </summary>
        public async Task<IEnumerable<Role>> GetRolesWithSpecificPermissionAsync(string name)
        {
            return await _context.Roles
                .Where(r => r.RolePermissions.Any(rp => rp.Permission.Name == name))
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .ToListAsync();
        }
    }
}
