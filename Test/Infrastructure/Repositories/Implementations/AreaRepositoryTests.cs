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
    public class AreaRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly AreaRepository _repository;

        public AreaRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(options);
            _repository = new AreaRepository(_context);

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

            var area = Area.Builder()
                .SetName("IT Department")
                .SetDescription("Handles IT operations")
                .AddRole(role)
                .AddProfile(profile)
                .Build();

            await _context.Roles.AddAsync(role);
            await _context.Profiles.AddAsync(profile);
            await _context.Areas.AddAsync(area);
            await _context.SaveChangesAsync();
        }

        [Fact]
        public async Task AddAsync_Should_Add_Area()
        {
            // Arrange
            var newArea = Area.Builder()
                .SetName("HR")
                .SetDescription("Human Resources")
                .Build();

            // Act
            await _repository.AddAsync(newArea);
            var retrievedArea = await _repository.GetByIdAsync(newArea.Id);

            // Assert
            Assert.NotNull(retrievedArea);
            Assert.Equal("HR", retrievedArea.Name);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_All_Areas()
        {
            // Act
            var areas = await _repository.GetAllAsync();

            // Assert
            Assert.Single(areas); // Porque solo agregamos una área en la BD de prueba
        }

        [Fact]
        public async Task GetAreasWithRolesAsync_Should_Return_Areas_With_Roles()
        {
            // Act
            var areas = await _repository.GetAreasWithRolesAsync();

            // Assert
            Assert.Single(areas);
            Assert.Single(areas.First().Roles); // Debe haber un solo rol en la primera área
        }

        [Fact]
        public async Task GetAreasWithProfilesAsync_Should_Return_Areas_With_Profiles()
        {
            // Act
            var areas = await _repository.GetAreasWithProfilesAsync();

            // Assert
            Assert.Single(areas);
            Assert.Single(areas.First().AreaProfiles); // Debe haber un solo perfil en la primera área
        }

        [Fact]
        public async Task ExistsAsync_Should_Return_True_If_Area_Exists()
        {
            // Arrange
            var area = await _context.Areas.FirstOrDefaultAsync();

            // Act
            var exists = await _repository.ExistsAsync(area.Id);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsAsync_Should_Return_False_If_Area_Does_Not_Exist()
        {
            // Act
            var exists = await _repository.ExistsAsync(Guid.NewGuid());

            // Assert
            Assert.False(exists);
        }
    }
}
