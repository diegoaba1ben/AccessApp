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
    public class UserRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(options);
            _repository = new UserRepository(_context);

            // Inicializar datos de prueba
            SeedDatabase().Wait();
        }

        private async Task SeedDatabase()
        {
            var role = Role.Builder()
                .SetName("Admin")
                .SetDescription("Administrator role")
                .Build();

            var profile = Profile.Builder()
                .WithRole(role)
                .Build();

            var user = User.Builder()
                .WithName("John Doe")
                .WithEmail("john.doe@example.com")
                .WithPassword("SecurePass123")
                .WithProfile(profile)
                .WithRoles(new List<Role> { role })
                .Build();

            await _context.Roles.AddAsync(role);
            await _context.Profiles.AddAsync(profile);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        [Fact]
        public async Task AddAsync_Should_Set_User_Active_By_Defoult()
        {
            // Arrange
            var newUser = User.Builder()
                .WithName("Test User")
                .WithEmail(Tests.user@example.com)
                .WithPassword("SecurePass123")
                .Build()

            // Act
            await _repository.AddAsync(newUser);
            var retrievedUser = await _repository.GetByIdAsync(newUser.Id);

            // Assert
            Assert.NotNull(retrievedUser);
            Assert.True(retrievedUser.IsActive);
        }

        [Fact]
        public async Task AddAsync_Should_Add_User()
        {
            // Arrange
            var newUser = User.Builder()
                .WithName("Jane Doe")
                .WithEmail("jane.doe@example.com")
                .WithPassword("SecurePass123")
                .Build();

            // Act
            await _repository.AddAsync(newUser);
            var retrievedUser = await _repository.GetByIdAsync(newUser.Id);

            // Assert
            Assert.NotNull(retrievedUser);
            Assert.Equal("Jane Doe", retrievedUser.Name);
        }

        [Fact]
        public async Task GetByEmailAsync_Should_Return_User()
        {
            // Act
            var user = await _repository.GetByEmailAsync("john.doe@example.com");

            // Assert
            Assert.NotNull(user);
            Assert.Equal("john.doe@example.com", user.Email);
        }

        [Fact]
        public async Task GetUserWithProfileAsync_Should_Return_User_With_Profile()
        {
            // Arrange
            var user = await _context.Users.FirstOrDefaultAsync();

            // Act
            var userWithProfile = await _repository.GetUserWithProfileAsync(user.Id);

            // Assert
            Assert.NotNull(userWithProfile);
            Assert.NotNull(userWithProfile.Profile);
        }

        [Fact]
        public async Task GetUserWithRolesAsync_Should_Return_User_With_Roles()
        {
            // Arrange
            var user = await _context.Users.FirstOrDefaultAsync();

            // Act
            var userWithRoles = await _repository.GetUserWithRolesAsync(user.Id);

            // Assert
            Assert.NotNull(userWithRoles);
            Assert.NotEmpty(userWithRoles.Roles);
        }

        [Fact]
        public async Task ExistsAsync_Should_Return_True_If_User_Exists()
        {
            // Arrange
            var user = await _context.Users.FirstOrDefaultAsync();

            // Act
            var exists = await _repository.ExistsAsync(user.Id);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsAsync_Should_Return_False_If_User_Does_Not_Exist()
        {
            // Act
            var exists = await _repository.ExistsAsync(Guid.NewGuid());

            // Assert
            Assert.False(exists);
        }
    }
}
