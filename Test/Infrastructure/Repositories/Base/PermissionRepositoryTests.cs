using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Infrastructure.Persistence;
using AccessAppUser.Infrastructure.Repositories.Implementations;
using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Tests.Infrastructure.Repositories.Implementations
{
    public class PermissionRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly PermissionRepository _repository;

        public PermissionRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(options);
            _repository = new PermissionRepository(_context);

            // Inicializar datos de prueba
            SeedDatabase().Wait();
        }

        private async Task SeedDatabase()
        {
            var permission1 = Permission.Builder()
                .WithName("Read")
                .WithDescription("Allows read access")
                .Build();

            var permission2 = Permission.Builder()
                .WithName("Write")
                .WithDescription("Allows write access")
                .Build();

            var role = Role.Builder()
                .SetName("Admin")
                .SetDescription("Administrator role")
                .AddPermissions(new List<Permission> { permission1, permission2 })
                .Build();

            await _context.Permissions.AddRangeAsync(permission1, permission2);
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }

        [Fact]
        public async Task AddAsync_Should_Add_Permission()
        {
            // Arrange
            var newPermission = Permission.Builder()
                .WithName("Delete")
                .WithDescription("Allows delete access")
                .Build();

            // Act
            await _repository.AddAsync(newPermission);
            var retrievedPermission = await _repository.GetByIdAsync(newPermission.Id);

            // Assert
            Assert.NotNull(retrievedPermission);
            Assert.Equal("Delete", retrievedPermission.Name);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_All_Permissions()
        {
            // Act
            var permissions = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, permissions.Count()); // Porque hay 2 permisos en la BD de prueba
        }

        [Fact]
        public async Task GetPermissionsByRoleIdAsync_Should_Return_Permissions()
        {
            // Arrange
            var role = await _context.Roles.Include(r => r.Permissions).FirstOrDefaultAsync(r => r.Name == "Admin");

            // Act
            var permissions = await _repository.GetPermissionsByRoleIdAsync(role.Id);

            // Assert
            Assert.Equal(2, permissions.Count());
        }

        [Fact]
        public async Task GetPermissionsByRoleNameAsync_Should_Return_Permissions()
        {
            // Act
            var permissions = await _repository.GetPermissionsByRoleNameAsync("Admin");

            // Assert
            Assert.Equal(2, permissions.Count());
        }

        [Fact]
        public async Task ExistsAsync_Should_Return_True_If_Permission_Exists()
        {
            // Arrange
            var permission = await _context.Permissions.FirstOrDefaultAsync();

            // Act
            var exists = await _repository.ExistsAsync(permission.Id);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsAsync_Should_Return_False_If_Permission_Does_Not_Exist()
        {
            // Act
            var exists = await _repository.ExistsAsync(Guid.NewGuid());

            // Assert
            Assert.False(exists);
        }
    }
}
