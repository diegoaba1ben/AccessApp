using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Persistence;
using AccessAppUser.Infrastructure.Repositories.Base;
using AccessAppUser.Infrastructure.Repositories.Interfaces;
using AccessAppUser.Infrastructure.Exceptions;


namespace AccessAppUser.Infrastructure.Repositories.Implementations
{
    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(AppDbContext context) : base(context) { }

        /// <summary>
        /// Obtiene los permisos asociados a un rol usando su ID.
        /// </summary>
        public async Task<IEnumerable<Permission>> GetPermissionByRoleIdAsync(Guid roleId)
        {
            var role = await _context.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Id == roleId);
            if (role is null)
            {
                throw new RoleNotFoundException(roleId);
            }
            return role?.Permissions ?? new List<Permission>();
        }

        /// <summary>
        /// Obtiene los permisos asociados a un rol usando su nombre.
        /// </summary>
        public async Task<IEnumerable<Permission>> GetPermissionByRoleNameAsync(string roleName)
        {
            var role = await _context.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Name == roleName.Trim());
            if (role is null)
            {
                throw new RoleNameNotFoundException(roleName);
            }

            return role?.Permissions ?? new List<Permission>();
        }
    }
}