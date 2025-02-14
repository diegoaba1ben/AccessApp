using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Infrastructure.Persistence;
using AccessAppUser.Infrastructure.Queries.Implementations;
using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Tests.Infrastructure.Queries.Implementations
{
    public class PermissionQueriesTests
    {
        private readonly AppDbContext _context;
        private readonly PermissionQueries _queries;

        public PermissionQueriesTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(options);
            _queries = new PermissionQueries(_context);

            // Inicializar datos de prueba
            SeedDatabase().Wait();
        }

        private async Task SeedDatabase()
        {
            // Crear permisos
            var readPermission = Permission.Builder()
                .WithName("Read")
                .WithDescription("Permiso para leer datos")
                .Build();

            var writePermission = Permission.Builder()
                .WithName("Write")
                .WithDescription("Permiso para escribir datos")
                .Build();

            // Crear roles
            var adminRole = Role.Builder()
                .SetName("Admin")
                .SetDescription("Administrador del sistema")
                .AddPermissions(new List<Permission> { readPermission, writePermission })
                .Build();

            var userRole = Role.Builder()
                .SetName("User")
                .SetDescription("Rol de usuario estándar")
                .AddPermissions(new List<Permission> { readPermission })
                .Build();

            // Agregar permisos a la base de datos
            await _context.Permissions.AddRangeAsync(readPermission, writePermission);
            await _context.Roles.AddRangeAsync(adminRole, userRole);
            await _context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetAllPermissionsAsync_Should_Return_All_Permissions()
        {
            // Act
            var permissions = await _queries.GetAllPermissionsAsync();

            // Assert
            Assert.NotEmpty(permissions);
            Assert.Contains(permissions, p => p.Name == "Read");
            Assert.Contains(permissions, p => p.Name == "Write");
        }

        [Fact]
        public async Task GetPermissionsByRoleAsync_Should_Return_Correct_Permissions()
        {
            // Act
            var permissions = await _queries.GetPermissionsByRoleAsync("Admin");

            // Assert
            Assert.NotEmpty(permissions);
            Assert.Equal(2, permissions.Count());
            Assert.Contains(permissions, p => p.Name == "Read");
            Assert.Contains(permissions, p => p.Name == "Write");
        }

        [Fact]
        public async Task GetRolesWithPermissionAsync_Should_Return_Correct_Roles()
        {
            // Act
            var roles = await _queries.GetRolesWithPermissionAsync("Read");

            // Assert
            Assert.NotEmpty(roles);
            Assert.Contains(roles, r => r.Name == "Admin");
            Assert.Contains(roles, r => r.Name == "User");
        }
    }
}
