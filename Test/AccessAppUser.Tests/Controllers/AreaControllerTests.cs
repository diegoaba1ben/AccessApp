using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Application.Controllers;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Repositories.Implementations;
using AccessAppUser.Infrastructure.Persistence;
using AccessAppUser.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccessAppUser.Tests.Controllers
{
    public class AreaControllerTests
    {
        private readonly AppDbContext _context;
        private readonly AreaRepository _repository;
        private readonly AreaController _controller;

        public AreaControllerTests()
        {
            // Configuración de InMemoryDatabase
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Area")
                .Options;

            _context = new AppDbContext(options);
            _repository = new AreaRepository(_context);
            _controller = new AreaController(_repository);
        }

        // Prueba para obtener todas las áreas
        [Fact]
        public async Task GetAllAreas_ReturnsOk_WithAreas()
        {
            // Arrange
            _context.Areas.RemoveRange(_context.Areas); // Limpiar la BD antes de la prueba
            await _context.SaveChangesAsync();

            var areas = new List<Area>
            {
                new Area { Id = Guid.NewGuid(), Name = "Finance", Description = "Finance Department" },
                new Area { Id = Guid.NewGuid(), Name = "HR", Description = "Human Resources" }
            };

            _context.Areas.AddRange(areas);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<GenericResponseDTO<IEnumerable<Area>>>(okResult.Value);

            Assert.NotNull(response.Data);
            Assert.Equal(2, response.Data!.Count());
        }

        // Prueba para obtener un área por ID
        [Fact]
        public async Task GetAreaById_ReturnsOk_WhenAreaExists()
        {
            // Arrange
            var area = new Area { Id = Guid.NewGuid(), Name = "IT", Description = "IT Department" };

            _context.Areas.Add(area);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetById(area.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<GenericResponseDTO<Area>>(okResult.Value);

            Assert.NotNull(response.Data);
            Assert.Equal(area.Id, response.Data.Id);
        }

        // Prueba para obtener un área que no existe
        [Fact]
        public async Task GetAreaById_ReturnsNotFound_WhenAreaDoesNotExist()
        {
            // Act
            var result = await _controller.GetById(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        // Prueba para crear un área
        [Fact]
        public async Task CreateArea_ReturnsCreatedAtAction_WhenValid()
        {
            // Arrange
            var area = new Area { Id = Guid.NewGuid(), Name = "Marketing", Description = "Marketing Department" };

            // Act
            var result = await _controller.Create(area);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var response = Assert.IsType<GenericResponseDTO<Area>>(createdAtResult.Value);

            Assert.NotNull(response.Data);
            Assert.Equal(area.Name, response.Data.Name);
        }

        // Prueba para eliminar un área existente

        [Fact]
        public async Task DeleteArea_ReturnsOk_WhenAreaExists()
        {
            // Arrange
            var area = new Area { Id = Guid.NewGuid(), Name = "Legal", Description = "Legal Department" };

            _context.Areas.Add(area);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Delete(area.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<GenericResponseDTO<Area>>(okResult.Value); // ✅ Se espera `GenericResponseDTO<Area>`

            Assert.NotNull(response.Data); // Aseguramos que `Data` no sea null
            Assert.Equal(area.Id, response.Data!.Id); // Comparamos `Id`
        }

        // Prueba para eliminar un área que no existe
        [Fact]
        public async Task DeleteArea_ReturnsNotFound_WhenAreaDoesNotExist()
        {
            // Act
            var result = await _controller.Delete(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        // Prueba para obtener áreas con roles
        [Fact]
        public async Task GetAreasWithRoles_ReturnsOk_WithData()
        {
            // Arrange
            _context.Areas.RemoveRange(_context.Areas); // Limpiar la BD antes de la prueba
            await _context.SaveChangesAsync();

            var role = new Role { Id = Guid.NewGuid(), Name = "Manager", Description = "Manager Role" };
            var area = new Area { Id = Guid.NewGuid(), Name = "Operations", Description = "Operations Department", Roles = new List<Role> { role } };

            _context.Areas.Add(area);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAreasWithRolesAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.Contains(result, a => a.Roles.Any(r => r.Name == "Manager"));
        }

        // Prueba para obtener áreas con perfiles
        [Fact]
        public async Task GetAreasWithProfiles_ReturnsOk_WithData()
        {
            // Arrange
            _context.Areas.RemoveRange(_context.Areas); // Limpiar la BD antes de la prueba
            await _context.SaveChangesAsync();

            var profile = new Profile { Id = Guid.NewGuid()}; 

            // Creación del área
            var area = new Area
             { 
                Id = Guid.NewGuid(),
                Name = "Security", 
                Description = "Security Department"
             };

            // Construcción de AreaProfile con el área inicializada
            var areaProfile = AreaProfile.Builder()
                .WithArea(area)
                .WithProfile(profile)
                .Build();

            // Asigna AreaProfile a area
            area.AreaProfiles = new List<AreaProfile> {areaProfile};

            _context.Areas.Add(area);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAreasWithProfilesAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.Contains(result, a => a.AreaProfiles.Any(ap => ap.Profile.Id != Guid.Empty));

        }
    }
}
