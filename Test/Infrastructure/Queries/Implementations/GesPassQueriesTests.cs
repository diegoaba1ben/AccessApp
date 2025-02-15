using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Persistence;
using AccessAppUser.Infrastructure.Queries.Implementations;

namespace AccessAppUser.Tests.Infrastructure.Queries
{
    public class GesPassQueriesTests
    {
        private readonly AppDbContext _context;
        private readonly GesPassQueries _gesPassQueries;

        public GesPassQueriesTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestGesPassDatabase")
                .Options;

            _context = new AppDbContext(options);
            _gesPassQueries = new GesPassQueries(_context);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var user = User.Builder()
                .WithName("Juan Perez")
                .WithEmail("juan.perez@example.com")
                .WithPassword("Password123")
                .Build();

            var gesPass1 = GesPass.Builder()
                .WithUser(user)
                .WithResetToken("token123", DateTime.UtcNow.AddHours(2))
                .Build();

            var gesPass2 = GesPass.Builder()
                .WithUser(user)
                .WithResetToken("expiredToken", DateTime.UtcNow.AddHours(-1))
                .Build();

            _context.Users.Add(user);
            _context.GesPasses.AddRange(gesPass1, gesPass2);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllGesPassesAsync_ShouldReturnAllRecords()
        {
            var result = await _gesPassQueries.GetAllGesPassesAsync();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetGesPassByUserIdAsync_ShouldReturnCorrectRecord()
        {
            var userId = _context.Users.First().Id;
            var result = await _gesPassQueries.GetGesPassByUserIdAsync(userId);
            result.Should().NotBeNull();
            result?.UserId.Should().Be(userId);
        }

        [Fact]
        public async Task GetGesPassByTokenAsync_ShouldReturnCorrectRecord()
        {
            var result = await _gesPassQueries.GetGesPassByTokenAsync("token123");
            result.Should().NotBeNull();
            result?.ResetToken.Should().Be("token123");
        }

        [Fact]
        public async Task GetGesPassesWithExpiredTokensAsync_ShouldReturnExpiredTokens()
        {
            var result = await _gesPassQueries.GetGesPassesWithExpiredTokensAsync();
            result.Should().HaveCount(1);
            result.First().ResetToken.Should().Be("expiredToken");
        }

        [Fact]
        public async Task GetAllPasswordsRequestsAsync_ShouldReturnAllRecords()
        {
            var result = await _gesPassQueries.GetAllPasswordsRequestsAsync();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetPasswordRequestByUserMailAsync_ShouldReturnCorrectRecord()
        {
            var result = await _gesPassQueries.GetPasswordRequestByUserMailAsync("juan.perez@example.com");
            result.Should().NotBeNull();
            result?.User.Email.Should().Be("juan.perez@example.com");
        }
    }
}
