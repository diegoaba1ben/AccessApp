using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.DTOs.GesPass;
using AccessAppUser.Infrastructure.Persistence;

namespace AccessAppUser.Tests.DTOsTest.Integracion
{
    public class GesPassDTOIntegrationTest
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_GesPass")
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task GesPassDTO_Should_SaveAndRetrieveWithUser()
        {
            // Arrange
            using var context = GetDbContext();

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Email = "testuser@example.com",
                Password = "SecurePass123!"
            };

            var gesPass = new GesPass
            {
                Id = Guid.NewGuid(),
                User = user,  // 🔹 Relación con el usuario
                UserId = user.Id,
                ResetToken = "ABC123",
                TokenExpiration = DateTime.UtcNow.AddHours(1),
                IsCompleted = false
            };

            // Act
            context.Users.Add(user);
            context.GesPasses.Add(gesPass);
            await context.SaveChangesAsync();

            var retrievedGesPass = await context.GesPasses
                .Include(gp => gp.User)
                .FirstOrDefaultAsync(gp => gp.Id == gesPass.Id);

            // Assert
            retrievedGesPass.Should().NotBeNull();
            retrievedGesPass!.Id.Should().Be(gesPass.Id);
            retrievedGesPass.User.Should().NotBeNull();
            retrievedGesPass.User!.Name.Should().Be(user.Name);
            retrievedGesPass.ResetToken.Should().Be(gesPass.ResetToken);
            retrievedGesPass.TokenExpiration.Should().BeCloseTo(gesPass.TokenExpiration, TimeSpan.FromSeconds(1));
            retrievedGesPass.IsCompleted.Should().BeFalse();
        }
    }
}

