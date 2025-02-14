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
    public class UserQueriesTests
    {
        private readonly AppDbContext _context;
        private readonly UserQueries _queries;

        public UserQueriesTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(options);
            _queries = new UserQueries(_context);

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

            // Crear un usuario
            var user = User.Builder()
                .WithName("Carlos Gómez")
                .WithEmail("carlos.gomez@example.com")
                .WithPassword("Password123")
                .WithIsActive(true)
                .WithRoles(new List<Role> { role })
                .Build();

            // Asignar un GesPass
            var gesPass = GesPass.Builder()
                .WithUser(user)
                .WithResetToken("token123", DateTime.UtcNow.AddHours(2))
                .Build();

            await _context.Roles.AddAsync(role);
            await _context.Users.AddAsync(user);
            await _context.GesPasses.AddAsync(gesPass);
            await _context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetAllUsersAsync_Should_Return_All_Users()
        {
            var users = await _queries.GetAllUsersAsync();
            Assert.NotEmpty(users);
            Assert.Equal("Carlos Gómez", users.First().Name);
        }

        [Fact]
        public async Task GetUserDetailsByEmailAsync_Should_Return_Correct_User()
        {
            var user = await _queries.GetUserDetailsByEmailAsync("carlos.gomez@example.com");
            Assert.NotNull(user);
            Assert.Equal("Carlos Gómez", user.Name);
        }

        [Fact]
        public async Task GetUsersByRoleAsync_Should_Return_Users_With_Specified_Role()
        {
            var users = await _queries.GetUsersByRoleAsync("Admin");
            Assert.NotEmpty(users);
            Assert.Equal("Admin", users.First().Roles.First().Name);
        }

        [Fact]
        public async Task GetUsersByStatusAsync_Should_Return_Active_Users()
        {
            var activeUsers = await _queries.GetUsersByStatusAsync(true);
            Assert.NotEmpty(activeUsers);
            Assert.True(activeUsers.All(u => u.IsActive));
        }

        [Fact]
        public async Task GetUserWithGesPassAsync_Should_Return_User_With_GesPass()
        {
            var user = await _queries.GetUserWithGesPassAsync("carlos.gomez@example.com");
            Assert.NotNull(user);
            Assert.NotNull(user?.GesPass);
            Assert.Equal("token123", user?.GesPass?.ResetToken);
        }
    }
}
