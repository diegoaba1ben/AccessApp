using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Persistence;

namespace AccessAppUser.Tests.Persistence  
{
    /// <summary>
    /// Pruebas de integración para verificar que los permisos de un rol se guardan correctamente en la base de datos.
    /// </summary>
    public class RolePermissionDbTest
    {
        [Fact]
        public void Role_Should_Persist_Permissions_In_Database()
        {
            // Configurar un contexto de base de datos en memoria
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new AppDbContext(options))
            {
                // Arrange: Crear permisos y un rol
                var perm1 = Permission.Builder()
                    .WithName("CreateUser")
                    .WithDescription("Permite crear usuarios")
                    .Build();

                var perm2 = Permission.Builder()
                    .WithName("DeleteUser")
                    .WithDescription("Permite eliminar usuarios")
                    .Build();

                var role = Role.Builder()
                    .SetName("Admin")
                    .SetDescription("Rol administrativo")
                    .AddPermissions(new List<Permission> { perm1, perm2 })
                    .Build();

                // Act: Guardar en la base de datos
                context.Permissions.AddRange(perm1, perm2);
                context.Roles.Add(role);
                context.SaveChanges();

                // Recuperar el rol desde la base de datos
                var retrievedRole = context.Roles
                    .Include(r => r.Permissions) // Incluir los permisos
                    .FirstOrDefault(r => r.Name == "Admin");

                // Assert: Validar que el rol tiene los permisos asignados correctamente
                Assert.NotNull(retrievedRole);
                Assert.Equal(2, retrievedRole.Permissions.Count);
                Assert.Contains(retrievedRole.Permissions, p => p.Name == "CreateUser");
                Assert.Contains(retrievedRole.Permissions, p => p.Name == "DeleteUser");
            }
        }
    }
}
