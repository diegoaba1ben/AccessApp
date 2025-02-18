using Xunit;
using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using AccessAppUser.Application.Controllers;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Repositories.Interfaces;
using AccessAppUser.Infrastructure.DTOs;
using AccessAppUser.Infrastructure.DTOs.Role;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
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
        [Fact]
        public async Task GetAllRoles_ReturnsOk_WithRoles()
        {
            // Limpia la base de datos antes de cada prueba
            _context.Roles.RemoveRange(_context.Roles);
            await _context.SaveChangesAsync();

            var roles = new List<Role>
            {
                new Role { Id = Guid.NewGuid(), Name = "Admin", Description = "Admin role" },
                new Role { Id = Guid.NewGuid(), Name = "User", Description = "User role" }
            };

            _context.Roles.AddRange(roles);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<GenericResponseDTO<IEnumerable<Role>>>(okResult.Value);

            Assert.NotNull(response.Data);
            Assert.Equal(2, response.Data!.Count()); 
        }


        // Prueba para obtener un rol por ID (heredado de BaseController<T>)
        [Fact]
        public async Task GetRoleById_ReturnsNotFound_WhenRoleDoesNotExist()
        {
            // Act
            var result =await _controller.GetById(Guid.NewGuid());

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
            var role = new Role {Id = Guid.NewGuid(), Name = "Manager", Description = "Manages teams"};
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
