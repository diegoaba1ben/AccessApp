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
    public class ProfileRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly ProfileRepository _repository;

        public ProfileRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(options);
            _repository = new ProfileRepository(_context);

            // Inicializar datos de prueba
            SeedDatabase().Wait();
        }

        private async Task SeedDatabase()
        {
            var role = Role.Builder()
                .SetName("Admin")
                .SetDescription("Administrator role")
                .Build();

            var area = Area.Builder()
                .SetName("IT")
                .SetDescription("Technology department")
                .Build();

            var profile = Profile.Builder()
                .WithRole(role)
                .WithAreaProfiles(new List<AreaProfile>
                {
                    AreaProfile.Builder()
                        .WithArea(area)
                        .WithProfile(profile)
                        .Build()
                })
                .Build();

            await _context.Roles.AddAsync(role);
            await _context.Areas.AddAsync(area);
            await _context.Profiles.AddAsync(profile);
            await _context.SaveChangesAsync();
        }

        [Fact]
        public async Task AddAsync_Should_Add_Profile()
        {
            // Arrange
            var newProfile = Profile.Builder()
                .WithRole(Role.Builder().SetName("Editor").SetDescription("Editor role").Build())
                .Build();

            // Act
            await _repository.AddAsync(newProfile);
            var retrievedProfile = await _repository.GetByIdAsync(newProfile.Id);

            // Assert
            Assert.NotNull(retrievedProfile);
        }

        [Fact]
        public async Task GetProfileWithUserAsync_Should_Return_Profile_With_User()
        {
            // Arrange
            var profile = await _context.Profiles.FirstOrDefaultAsync();

            // Act
            var profileWithUser = await _repository.GetProfileWithUserAsync(profile.Id);

            // Assert
            Assert.NotNull(profileWithUser);
            Assert.NotNull(profileWithUser.User);
        }

        [Fact]
        public async Task GetProfileWithRoleAsync_Should_Return_Profile_With_Role()
        {
            // Arrange
            var profile = await _context.Profiles.FirstOrDefaultAsync();

            // Act
            var profileWithRole = await _repository.GetProfileWithRoleAsync(profile.Id);

            // Assert
            Assert.NotNull(profileWithRole);
            Assert.NotNull(profileWithRole.Role);
        }

        [Fact]
        public async Task GetProfileWithAreasAsync_Should_Return_Profile_With_Areas()
        {
            // Arrange
            var profile = await _context.Profiles.FirstOrDefaultAsync();

            // Act
            var profileWithAreas = await _repository.GetProfileWithAreasAsync(profile.Id);

            // Assert
            Assert.NotNull(profileWithAreas);
            Assert.NotEmpty(profileWithAreas.AreaProfiles);
        }

        [Fact]
        public async Task ExistsAsync_Should_Return_True_If_Profile_Exists()
        {
            // Arrange
            var profile = await _context.Profiles.FirstOrDefaultAsync();

            // Act
            var exists = await _repository.ExistsAsync(profile.Id);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsAsync_Should_Return_False_If_Profile_Does_Not_Exist()
        {
            // Act
            var exists = await _repository.ExistsAsync(Guid.NewGuid());

            // Assert
            Assert.False(exists);
        }
    }
}
