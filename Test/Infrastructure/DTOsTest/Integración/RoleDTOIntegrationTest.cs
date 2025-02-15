using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.DTOs.Role;
using AccessAppUser.Infrastructure.Persistence;

namespace AccessAppUser.Tests.DTOsTest.Integracion
{
    public class RoleDTOIntegrationTest
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_Role")
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task RoleDTO_Should_SaveAndRetrieveCorrectly()
        {
            // Arrange
            using var context = GetDbContext();
            var role = new Role
            {
                Id = Guid.NewGuid(),
                Name = "Test Role",
                Description = "Role for testing purposes"
            };

            // Act
            context.Roles.Add(role);
            await context.SaveChangesAsync();

            var retrievedRole = await context.Roles.FirstOrDefaultAsync(r => r.Id == role.Id);

            // Assert
            retrievedRole.Should().NotBeNull();
            retrievedRole!.Id.Should().Be(role.Id);
            retrievedRole.Name.Should().Be(role.Name);
            retrievedRole.Description.Should().Be(role.Description);
        }
    }
}