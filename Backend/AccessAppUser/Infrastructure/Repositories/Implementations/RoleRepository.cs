using Microsoft.EntityFrameworkCore;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Persistence;
using AccessAppUser.Infrastructure.Repositories.Base;
using AccessAppUser.Infrastructure.Repositories.Interfaces;

namespace AccessAppUser.Infrastructure.Repositories.Implementations
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context) { }

        /// <summary>
        /// Obtiene todos los permisos asociados a un rol.
        /// </summary>
        public async Task<IEnumerable<Permission>> GetPermissionsByRoleIdAsync(Guid roleId)
        {
            var role = await _context.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Id == roleId);

            return role?.RolePermissions.Select(rp => rp.Permission) ?? new List<Permission>();
        }

        /// <summary>
        /// Asigna un permiso a un rol.
        /// </summary>
        public async Task<bool> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            var permission = await _context.Permissions.FindAsync(permissionId);

            if (role == null || permission == null)
                return false;

            _context.RolePermissions.Add(new RolePermission { RoleId = roleId, PermissionId = permissionId });
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Elimina un permiso de un rol.
        /// </summary>
        public async Task<bool> RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId)
        {
            var rolePermission = await _context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

            if (rolePermission == null)
                return false;

            _context.RolePermissions.Remove(rolePermission);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Obtiene todos los usuarios asociados a un rol.
        /// </summary>
        public async Task<IEnumerable<User>> GetUsersByRoleIdAsync(Guid roleId)
        {
            var role = await _context.Roles
                .Include(r => r.Users)
                .FirstOrDefaultAsync(r => r.Id == roleId);

            return role?.Users ?? new List<User>();
        }

        /// <summary>
        /// Obtiene todas las áreas asociadas a un rol.
        /// </summary>
        public async Task<IEnumerable<Area>> GetAreasByRoleIdAsync(Guid roleId)
        {
            var role = await _context.Roles
                .Include(r => r.Areas)
                .FirstOrDefaultAsync(r => r.Id == roleId);

            return role?.Areas ?? new List<Area>();
        }

        /// <summary>
        /// Obtiene los roles junto con sus permisos asociado
        /// </summary>
        public async Task<IEnumerable<Role>> GetRolesWithPermissionsAsync()
        {
            return await _context.Roles 
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .ToListAsync();
        }

        /// <summary>
        /// obtiene los roles junto con sus áreas asociadas
        /// </summary>
        public async Task<IEnumerable<Role>> GetRolesWithAreasAsync()
        {
            return await _context.Roles
                .Include(r => r.Areas)
                .ToListAsync();
        }
    }
}
