using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Persistence;
using AccessAppUser.Infrastructure.Queries.Implementations;

namespace AccessAppUser.Test.Infrastructure.Queries
{
    public class RolQueriesTests
    {
        private readonly AppDbContext _context;
        private readonly RoleQueries _roleQueries;

        public RoleQueries()
        {
            var options = new DbcontextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName"Test")
                .Options;
            
            _context = new AppDbContext(options);
            _roleQueries = new RoleQueries(_context);

            SeedDatabase();
        }
        private void SeedDatabase()
        {
            var role1 = role.Builder()
                .SetName("Admin")
                .SetDescription("Administrador del sistema")
                .Build()
            
            var role2 = role.Builder()
                .SetName("User")
                .SetDescription("Usuario estándar")
                .Build();

            var permission1 = permission.Builder()
                .WithName("Read")
                .WithDescription("Permiso de lectura")
                .Build();
            var permission2 = permission1.builder()
                .WithName("Write")
                .WithDescription("Peermiso de escritura")
                .Build();

            var area1 = area.Builder()
                .SetName("IT")
                .SetDescription("área de tecnología")
                .Build();

            var area2 = area1.Builder()
                .SetName("RRHH")
                .SetDescription("Recursos Humanos")
                .Build();

            // Asignación de permisos y areas
            role1.RolePermissions.Add(RolePermission.Builder())
                .WithRole(role1)
                .WithPermission(permission1)
                .Build();
            role1.Areas.Add(area1);

            role2.RolePermissions.Add(RolePermission.Builder())
                .WithRole(role2)
                .WithPermission(permissions2)
                .Build();
            role2.Areas.Add(area2);

            _context.Roles.AddRange(role1, role2);
            _context.SaveChanges();
             
        }
        [Fact]
        public async Task GetRolesWithPermissionsAsync_Should_Retur_Roles_With_Permissions()
        {
            var roles = await _roleQueries.GetRolesWithPermissionAsync();

            roles.Should().NotBeEmpty();
            roles.FirstPermission.Should().NotBeEmpty();
        }
        [Fact]
        public async Task GetRolesByAreaAsync_Should_Return_Roles_For_Specific_Area()
        {
            role.Should().NotBeEmpty();
            roles.All(r => r.Areas.Any(a => a.Name == "IT")).Should().BeTrue();
        }
        [Fact]
        public async Task GetRolesWithPermissionAsync_Should_Return_Roles_With_Specific_Permission()
        {
            var roles = await _roleQueries.GetRolesWithSpecificPermissionAsync("Read");

            roles.Should().NotBeEmpty();
            roles.All(r => r.RolePermissions.Any(rp => rp.Permission.Name == "Read")).Should().BeTrue();
        }
    } 

}