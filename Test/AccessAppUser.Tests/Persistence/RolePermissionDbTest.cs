using Xunit;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace AccessAppUser.Tests.Persistence
{
    public class RolePermissionDbTest
    {
        private readonly AppDbContext _context;

        public RolePermissionDbTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(options);
        }

        [Fact]
        public async Task Role_Should_Persist_Permissions_In_Database()
        {
            //  PASO 1: Verificar cuántos permisos hay antes de la prueba
            var permissionsBefore = await _context.Permissions.CountAsync();
            var rolePermissionsBefore = await _context.RolePermissions.CountAsync();
            Console.WriteLine($" Permisos antes de la prueba: {permissionsBefore}");
            Console.WriteLine($" RolePermissions antes de la prueba: {rolePermissionsBefore}");
            Console.Out.Flush();

            //  PASO 2: Crear permisos y un rol
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
                .Build(); //  NO agregamos permisos aquí, se hace en RolePermissions

            // PASO 3: Guardar en la base de datos
            _context.Permissions.AddRange(perm1, perm2);
            _context.Roles.Add(role);

            // Guardado explícito de la relación RolePermissions antes de salvar
            _context.RolePermissions.AddRange(
                new RolePermission { Role = role, Permission = perm1 },
                new RolePermission { Role = role, Permission = perm2 }
            );
            await _context.SaveChangesAsync(); // Guarda en BD

            // PASO 4: Recuperar el rol desde la base de datos incluyendo la relación intermedia
            var retrievedRole = await _context.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Name == "Admin");
            // PASO 5: Validar que el rol tiene los permisos asignados correctamente
            if (retrievedRole == null)
            {
                Console.WriteLine(" El rol no se encontró en la base de datos.");
            }
            else
            {
                Console.WriteLine($" Rol recuperado: {retrievedRole.Name}");
                Console.Out.Flush();

                Console.WriteLine($" Cantidad de RolePermissions vinculados: {retrievedRole.RolePermissions.Count}");

                // IMPRIMIR PERMISOS RECUPERADOS
                foreach (var rp in retrievedRole.RolePermissions)
                {
                    Console.WriteLine($" - Relación encontrada: RoleID={rp.RoleId}, PermissionID={rp.PermissionId}");
                    Console.WriteLine($"   → Permiso asociado: {rp.Permission.Name}");
                    Console.Out.Flush();
                }
            }

            // PASO 6: Verificar cuántos permisos hay después de la prueba
            var permissionsAfter = await _context.Permissions.CountAsync();
            Console.WriteLine($" Permisos después de la prueba: {permissionsAfter}");
            Console.Out.Flush();

            // PASO 7: Verificación final
            Assert.NotNull(retrievedRole);
            Assert.Equal(2, retrievedRole.RolePermissions.Count); // Deben existir 2 relaciones
            Assert.Contains(retrievedRole.RolePermissions, rp => rp.PermissionId == perm1.Id);
            Assert.Contains(retrievedRole.RolePermissions, rp => rp.PermissionId == perm2.Id);

        }
    }
}
