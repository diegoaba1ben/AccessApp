using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Infrastructure.Persistence;
using AccessAppUser.Infrastructure.Repositories.Implementations;
using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Tests.Infrastructure.Repositories.Implementations
{
    public class GesPassRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly GesPassRepository _repository;

        public GesPassRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(options);
            _repository = new GesPassRepository(_context);

            // Inicializar datos de prueba
            SeedDatabase().Wait();
        }

        private async Task SeedDatabase()
        {
            var user = User.Builder()
                .WithName("John Doe")
                .WithEmail("john.doe@example.com")
                .WithPassword("SecurePass123")
                .Build();

            var gesPass = GesPass.Builder()
                .WithUser(user)
                .WithResetToken("reset-token-123", DateTime.UtcNow.AddHours(1))
                .Build();

            await _context.Users.AddAsync(user);
            await _context.GesPasses.AddAsync(gesPass);
            await _context.SaveChangesAsync();
        }

        [Fact]
        public async Task AddAsync_Should_Add_GesPass()
        {
            // Arrange
            var user = await _context.Users.FirstOrDefaultAsync();
            var newGesPass = GesPass.Builder()
                .WithUser(user)
                .WithResetToken("new-reset-token", DateTime.UtcNow.AddHours(2))
                .Build();

            // Act
            await _repository.AddAsync(newGesPass);
            var retrievedGesPass = await _repository.GetByIdAsync(newGesPass.Id);

            // Assert
            Assert.NotNull(retrievedGesPass);
            Assert.Equal("new-reset-token", retrievedGesPass.ResetToken);
        }

        [Fact]
        public async Task GetByUserIdAsync_Should_Return_GesPass_For_User()
        {
            // Arrange
            var user = await _context.Users.FirstOrDefaultAsync();

            // Act
            var gesPass = await _repository.GetByUserIdAsync(user.Id);

            // Assert
            Assert.NotNull(gesPass);
            Assert.Equal(user.Id, gesPass.UserId);
        }

        [Fact]
        public async Task GetByResetTokenAsync_Should_Return_GesPass()
        {
            // Act
            var gesPass = await _repository.GetByResetTokenAsync("reset-token-123");

            // Assert
            Assert.NotNull(gesPass);
            Assert.Equal("reset-token-123", gesPass.ResetToken);
        }

        [Fact]
        public async Task ExistsAsync_Should_Return_True_If_GesPass_Exists()
        {
            // Arrange
            var gesPass = await _context.GesPasses.FirstOrDefaultAsync();

            // Act
            var exists = await _repository.ExistsAsync(gesPass.Id);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsAsync_Should_Return_False_If_GesPass_Does_Not_Exist()
        {
            // Act
            var exists = await _repository.ExistsAsync(Guid.NewGuid());

            // Assert
            Assert.False(exists);
        }
    }
}
