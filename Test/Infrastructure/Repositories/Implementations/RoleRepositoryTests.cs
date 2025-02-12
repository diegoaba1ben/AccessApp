using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Persistence;
using AccessAppUser.Infrastructure.Repositories.implementations;

namespace AccessAppUser.Tests.Infrastructure.Repositories.Implementations
{
    public class RoleRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly RoleRepository _repository;

        public RoleRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName:"TestDatabase")
                .Options;

            _context = new AppDbContext(options);
            _repository = new RoleRepository(_context)

            // Inicialización de la base de datos
            SeedDatabase().Wait();
        }
        private async Task SeedDatabase()
        {
            var permission1 = Permission.Builder()
                .WithName("Read")
                .WithDescription("Allows read access")
                .Build()

            var permission2 = permission2.Builder()
                .WithName("Write")
                .WithDescription("Allows write access")
                .Build()

            var role = Role.Builder()
                .WithSetName("Admin")
                .WithSetDescription("Administrator role")
                .AddPermissions(new List<Permission> {permission1, permission2})
                .Build()

            await _context.Permissions.AddRangeAsync(permission1,permission2);
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync()
        }
        [Fact]
        public async Task AddAsync_Should_Add_Role()
        {
            // Arrange
            var newRole = RoleRepositoryTests.Builder()
                .SetName("Editor")
                .setDescription("Editor role with limited access")
                .Build();

            // Act
            await _repository.AddAsync(newRole)
            var retrievedRole = await _repository.GetByIdAsync(newRole.Id);

            // Assert
            Assert.NotNull(retrievedRole);
            Assert.Equal("Editor", retrievedRole.Name);
        }
        [Fact]
        public async Task GetAllAsync_Should_Return_All_Roles()
        {
            // Act
            var roles = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(1, roles.count()); /7 Con solo un rol en la base de datos
        }
        [Fact]
        public async Task GetRolesWithPermissionsAsync_Should_Return_Roles_With_Permission()
        {
            // Act
            var roles = await _respository.GetRolesWithPermissionsAsync();

            // Assert
            Assert.Single(roles);
            Assert.Equal(2, roles.First().Permissions.Count);
        }
        [Fact]
        public async Task ExistsAsync_Should_Return_Tru_If_Role_Exists()
        {
            // Arrange
            var role = await _context.Roles.FirstOrDefaultAsync();

            // Act
            var exists = await _repository.ExistsAsync(role.Id)

            // Assert
            Assert.True(exists);
        }
        [Fact]
        public async Task ExistsAsync_Should_Return_False_If_Role_Does_Not_Exists()
        {
            // Act
            var exists = await _repository.ExistsAsync(Guid.NewGuid());

            // Assert
            Assert.False(exists);

        }
    }
}