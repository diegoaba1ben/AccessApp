using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.DTOs.User;
using AccessAppUser.Infrastructure.Persistence;

namespace AccessAppUser.Tests.DTOsTest.Integracion
{
    public class UserDTOIntegrationTest
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_User")
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task UserDTO_Should_SaveAndRetrieveWithRolesAndProfile()
        {
            // Arrange
            using var context = GetDbContext();

            var role = new Role { Id = Guid.NewGuid(), Name = "Admin", Description = "Administrator Role" };
            var profile = new Profile { Id = Guid.NewGuid(), Role = role, UserName = "TestUser" };

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                Email = "johndoe@example.com",
                Password = "SecurePass123!",
                IsActive = true,
                Profile = profile,
                Roles = new List<Role> { role }
            };

            // Act
            context.Roles.Add(role);
            context.Profiles.Add(profile);
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var retrievedUser = await context.Users
                .Include(u => u.Roles)
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            // Assert
            retrievedUser.Should().NotBeNull();
            retrievedUser!.Id.Should().Be(user.Id);
            retrievedUser.Name.Should().Be(user.Name);
            retrievedUser.Email.Should().Be(user.Email);
            retrievedUser.IsActive.Should().BeTrue();
            
            retrievedUser.Profile.Should().NotBeNull();
            retrievedUser.Profile!.UserName.Should().Be(profile.UserName);

            retrievedUser.Roles.Should().NotBeNull();
            retrievedUser.Roles.Should().HaveCount(1);
            retrievedUser.Roles[0].Name.Should().Be(role.Name);
        }
    }
}
