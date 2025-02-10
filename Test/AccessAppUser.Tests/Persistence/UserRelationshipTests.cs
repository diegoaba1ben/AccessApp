using System;
using System.Linq;
using System.Threading.Tasks;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AccessAppUser.Tests.Persistence
{
    /// <summary>
    /// Comprueba que un usuario puede tener roles.
    /// Garantiza que al eliminar un usuario, no se eliminen los roles asociados.
    /// </summary>
    public class UserRoleRelationshipTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_UserRole")
                .Options;

            return new AppDbContext(options);
        }
        [Fact]
        public async Task User_Should_Have_Role_Assigned()
        {
            using var context = GetDbContext();

            var role = Role.Builder()
                .SetName("Admin")
                .SetDescription("Rol con privilegios de administrador")
                .Build();

            var user = User.Builder()
                .WithName("John Doe")
                .WithEmail("diego@example.com")
                .WithPassword("SecurePass2024!")
                .WithRoles(new[] { role })
                .Build();

            context.Roles.Add(role);
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Act 
            var savedUser = await context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == user.Id);
            
            // Assert
            Assert.NotNull(savedUser);
            Assert.Single(savedUser.Roles);
            Assert.Equal("Admin", savedUser.Roles.First().Name);
        }
        [Fact]
        public async Task Deleting_User_Should_Not_Delete_Role()
        {
            // Arrange
            using var context = GetDbContext();

            var role = Role.Builder()
                .SetName("Admin")
                .SetDescription("Rol con privilegios de administrador")
                .Build();
            var user = User.Builder()
                .WithName("Carlos")
                .WithEmail("carlos@email.com")
                .WithPassword("SecurePass2024!")
                .WithRoles(new[] { role })
                .Build();

            context.Roles.Add(role);
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Act - Eliminación del usuario.
            context.Users.Remove(user);
            await context.SaveChangesAsync();

            var remainingRoles = await context.Roles.ToListAsync();
            var remainigUsers = await context.Users.ToListAsync();

            // Assert
            Assert.Empty(remainigUsers); // No hay usuarios
            Assert.Single(remainingRoles); // Solo queda el rol
            Assert.Equal("Admin", remainingRoles.First().Name);
        }
    }
}