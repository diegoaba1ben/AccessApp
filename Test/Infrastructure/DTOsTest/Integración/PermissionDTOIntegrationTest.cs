using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.DTOs.Permission;
using AccessAppUser.Infrastructure.Persistence;

namespace AccessAppUser.Tests.DTOsTest.Integracion
{
    public class PermissionDTOIntegrationTest
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_Permission")
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task PermissionDTO_Should_SaveAndRetrieveCorrectly()
        {
            // Arrange
            using var context = GetDbContext();
            var permission = new Permission
            {
                Id = Guid.NewGuid(),
                Name = "Test Permission",
                Description = "Permission for testing purposes"
            };

            // Act
            context.Permissions.Add(permission);
            await context.SaveChangesAsync();

            var retrievedPermission = await context.Permissions.FirstOrDefaultAsync(p => p.Id == permission.Id);

            // Assert
            retrievedPermission.Should().NotBeNull();
            retrievedPermission!.Id.Should().Be(permission.Id);
            retrievedPermission.Name.Should().Be(permission.Name);
            retrievedPermission.Description.Should().Be(permission.Description);
        }
    }
}
