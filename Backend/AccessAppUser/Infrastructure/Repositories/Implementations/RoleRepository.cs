using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Persistence;
using AccessAppUser.Infrastructure.Repositories.Base;
using AccessAppUser.Infrastructure.Repositories.Interfaces;
using AccessAppuser.Infrastruture.Repositories.Interfaces;

namespace AccessAppUser.Infrastructure.Repositories.Implementations
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context){ }

        /// <summary>
        ///  Obtiene todos los roles junto con sus permisos asociados
        /// </summary>
        /// <returns>Lista de roles con sus permisos asociados</returns>
        public async Task<IEnumerable<Role>> GetRolesWithPermissionsAsync()
        {
            return await _context.Roles
                .Include(r => r.Permissions)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene los roles junto con sus áreas asociadas
        /// </summary>
        /// <returns>Lista de roles con sus áreas asociadas</returns>
        public async Task<IEnumerable<Role>> GetRolesWithAreasAsync()
        {
            return await _context.Roles
                .Include(r => r.Areas)
                .ToListAsync();
        }
    }
}