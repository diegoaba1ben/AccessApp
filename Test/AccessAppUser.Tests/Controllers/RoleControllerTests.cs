using Xunit;
using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using AccessAppUser.Application.Controllers;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Repositories.Interfaces;
using AccessAppUser.Infrastructure.DTOs;
using AccessAppUser.Infrastructure.DTOs.Role;
using AccessAppUser.Infrastructure.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Infrastructure.Persistence;
using System.Data.Common;

namespace AccessAppUser.Tests.Controllers
{
    public class RoleControllerTests
    {
        private readonly AppDbContext _context; // Se usamos IBaseRepository<Role> en lugar de IRoleRepository
        private readonly RoleController _controller;
        private readonly RoleRepository _repository;

        public RoleControllerTests()
        {
            // configuración de InMemory

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(options);
            _repository = new RoleRepository(_context);
            _controller = new RoleController(_repository);
        }

        //  Prueba para obtener todos los roles (heredado de BaseController<T>)
        //  Prueba para obtener todos los roles (heredado de BaseController<T>)
        [Fact]
        public async Task GetAllRoles_ReturnsOk_WithRoles()
        {
            // PASO 1: Limpiar la base de datos
            Console.WriteLine(" Limpiando la base de datos antes de agregar roles...");
            int rolesBefore = await _context.Roles.CountAsync(); // Definición de la variable
            Console.WriteLine($" Roles en BD antes de limpiar: {rolesBefore}");

            _context.Roles.RemoveRange(_context.Roles);
            await _context.SaveChangesAsync();

            int rolesAfterClean = await _context.Roles.CountAsync();
            Console.WriteLine($" Roles en BD después de limpiar: {rolesAfterClean}");

            // PASO 2: Creación de roles de prueba.
            Console.WriteLine(" Agregando roles de prueba a la base de datos...");
            var roles = new List<Role>
    {
        new Role { Id = Guid.NewGuid(), Name = "Admin", Description = "Admin role"},
        new Role { Id = Guid.NewGuid(), Name = "User", Description = "User role"}
    };
            foreach (var role in roles)
            {
                Console.WriteLine($" Creando rol {role.Name} - ID: {role.Id}");
            }

            _context.Roles.AddRange(roles);
            await _context.SaveChangesAsync();

            int rolesAfterAdd = await _context.Roles.CountAsync();
            Console.WriteLine($" Roles en BD después de agregar: {rolesAfterAdd}");

            // PASO 3: Llamar al controlador
            var result = await _controller.GetAll();
            Console.WriteLine(" Se llama al método GetAll() del controlador");

            // PASO 4: Validar respuesta
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Console.WriteLine("  El resultado es OkObjectResult");

            var response = Assert.IsType<GenericResponseDTO<IEnumerable<Role>>>(okResult.Value);

            // Validar si response.Data no es nulo
            if (response.Data == null)
            {
                Console.WriteLine(" ERROR: response.Data es nulo");
                Assert.Fail("response.Data es nulo");
            }

            Console.WriteLine($" Datos devueltos: {response.Data.Count()} roles");

            foreach (var role in response.Data!)
            {
                Console.WriteLine($"  Rol en respuesta: {role.Id}, Nombre: {role.Name}");
            }

            // PASO 5: Verificación final.
            Assert.NotNull(response.Data);
            Assert.Equal(2, response.Data!.Count());
        }


        // Prueba para obtener un rol por ID (heredado de BaseController<T>)
        [Fact]
        public async Task GetRoleById_ReturnsNotFound_WhenRoleDoesNotExist()
        {
            // Act
            var result = await _controller.GetById(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        //  Prueba para la creación de roles (heredado de BaseController<T>)
        [Fact]
        public async Task CreateRole_ReturnsCreatedAtAction_WhenValid()
        {
            // Arrange
            var role = new Role { Id = Guid.NewGuid(), Name = "Manager", Description = "Manages teams" };

            // Act
            var result = await _controller.Create(role);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var response = Assert.IsType<GenericResponseDTO<Role>>(createdAtResult.Value);
            Assert.Equal(role.Name, response.Data!.Name);
        }

        // Prueba para eliminar un rol
        [Fact]
        public async Task DeleteRole_ReturnsOk_WhenRoleExists()
        {
            // Arrange
            var role = new Role { Id = Guid.NewGuid(), Name = "Manager", Description = "Manages teams" };
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Delete(role.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<GenericResponseDTO<Role>>(okResult.Value);
            Assert.Equal(200, response.StatusCode);
        }

        //  Prueba para eliminar un rol que no existe
        [Fact]
        public async Task DeleteRole_ReturnsNotFound_WhenRoleDoesNotExist()
        {
            // Act
            var result = await _controller.Delete(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
    }
}
