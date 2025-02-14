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
    public class ProfileQueriesTests
    {
        private readonly AppDbContext _context;
        private readonly ProfileQueries _queries;

        public ProfileQueriesTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(options);
            _queries = new ProfileQueries(_context);

            // Inicializar datos de prueba
            SeedDatabase().Wait();
        }

        private async Task SeedDatabase()
        {
            // Crear un rol
            var role = Role.Builder()
                .SetName("Admin")
                .SetDescription("Administrador del sistema")
                .Build();

            // Crear un área
            var area = Area.Builder()
                .SetName("Tecnología")
                .SetDescription("Área de IT")
                .Build();

            // Crear un usuario
            var user = User.Builder()
                .WithName("Juan Pérez")
                .WithEmail("juan.perez@example.com")
                .WithPassword("Password123")
                .WithIsActive(true)
                .Build();

            // Crear un perfil asociado
            var profile = Profile.Builder()
                .WithUser(user)
                .WithRole(role)
                .Build();

            // Crear la relación con el área
            var areaProfile = AreaProfile.Builder()
                .WithArea(area)
                .WithProfile(profile)
                .Build();

            await _context.Roles.AddAsync(role);
            await _context.Areas.AddAsync(area);
            await _context.Users.AddAsync(user);
            await _context.Profiles.AddAsync(profile);
            await _context.AreaProfiles.AddAsync(areaProfile);
            await _context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetAllProfilesAsync_Should_Return_All_Profiles()
        {
            // Act
            var profiles = await _queries.GetAllProfilesAsync();

            // Assert
            Assert.NotEmpty(profiles);
            Assert.Single(profiles);
            Assert.Equal("Juan Pérez", profiles.First().User.Name);
        }

        [Fact]
        public async Task GetProfilesByStatusAsync_Should_Return_Active_Profiles()
        {
            // Act
            var activeProfiles = await _queries.GetProfilesByStatusAsync(true);

            // Assert
            Assert.NotEmpty(activeProfiles);
            Assert.True(activeProfiles.All(p => p.User.IsActive));
        }

        [Fact]
        public async Task GetProfilesByStatusAsync_Should_Return_Inactive_Profiles()
        {
            // Arrange
            var user = await _context.Users.FirstAsync();
            user.GetType().GetProperty("IsActive")?.SetValue(user, false);
            await _context.SaveChangesAsync();

            // Act
            var inactiveProfiles = await _queries.GetProfilesByStatusAsync(false);

            // Assert
            Assert.NotEmpty(inactiveProfiles);
            Assert.True(inactiveProfiles.All(p => !p.User.IsActive));
        }

        [Fact]
        public async Task GetProfileDetailsByUserEmailAsync_Should_Return_Correct_Profile()
        {
            // Act
            var profile = await _queries.GetProfileDetailsByUserEmailAsync("juan.perez@example.com");

            // Assert
            Assert.NotNull(profile);
            Assert.Equal("Juan Pérez", profile.User.Name);
            Assert.Equal("Admin", profile.Role.Name);
        }

        [Fact]
        public async Task GetProfilesByRoleAsync_Should_Return_Correct_Profiles()
        {
            // Act
            var profiles = await _queries.GetProfilesByRoleAsync("Admin");

            // Assert
            Assert.NotEmpty(profiles);
            Assert.Equal("Admin", profiles.First().Role.Name);
        }

        [Fact]
        public async Task GetProfilesByAreaAsync_Should_Return_Correct_Profiles()
        {
            // Act
            var profiles = await _queries.GetProfilesByAreaAsync("Tecnología");

            // Assert
            Assert.NotEmpty(profiles);
            Assert.Equal("Tecnología", profiles.First().AreaProfiles.First().Area.Name);
        }
    }
}