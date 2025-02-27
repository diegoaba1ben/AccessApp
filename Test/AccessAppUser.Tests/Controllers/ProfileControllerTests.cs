using Xunit;
using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Application.Controllers;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Repositories.Interfaces;
using AccessAppUser.Infrastructure.Repositories.Implementations;
using AccessAppUser.Infrastructure.Persistence;
using System;
using ProfileEntity = AccessAppUser.Domain.Entities.Profile;
using AccessAppUser.Infrastructure.DTOs;

namespace AccessAppUser.Tests.Controllers
{
    public class ProfileControllerTests
    {
        private readonly AppDbContext _context;
        private readonly ProfileRepository _repository;
        private readonly ProfileController _controller;
        private readonly Mock<IMapper> _mockMapper;

        public ProfileControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(options);
            _repository = new ProfileRepository(_context);
            _mockMapper = new Mock<IMapper>();
            _controller = new ProfileController(_repository);
        }

        /// 🔹 **Prueba: Obtener todos los perfiles**
        [Fact]
        public async Task GetAllProfiles_ReturnsOk_WithProfiles()
        {
            // Arrange
            _context.Profiles.RemoveRange(_context.Profiles);
            await _context.SaveChangesAsync();

            var role = Role.Builder().SetName("Admin").SetDescription("Admin Role").Build();
            var user1 = User.Builder().WithName("Alice").WithEmail("alice@example.com").WithRoles(new List<Role>{role}).Build();
            var user2 = User.Builder().WithName("Bob").WithEmail("bob@example.com").WithRoles(new List<Role>{role}).Build();
            
            var profiles = new List<ProfileEntity> 
            {
                ProfileEntity.Builder().WithUser(user1).WithRole(role).Build(),
                ProfileEntity.Builder().WithUser(user2).WithRole(role).Build()
            };

            _context.Roles.Add(role);
            _context.Users.AddRange(user1, user2);
            _context.Profiles.AddRange(profiles);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var responseDTO = Assert.IsType<GenericResponseDTO<IEnumerable<ProfileEntity>>>(okResult.Value);
            var response = responseDTO.Data;

            Assert.NotNull(response); // Verifica que la línea no sea nula
            Assert.Equal(2, response.Count()); // Valida la cantidad de perfiles obtenidos
        }

        ///  **Prueba: Obtener perfil por ID (existe)**
        [Fact]
        public async Task GetProfileById_ReturnsOk_WhenProfileExists()
        {
            // Arrange
            var role = Role.Builder().SetName("Manager").SetDescription("Manager Role").Build();
            var user = User.Builder().WithName("Charlie").WithEmail("charlie@example.com").WithRoles(new List<Role>{role}).Build();
            var profile = ProfileEntity.Builder().WithUser(user).WithRole(role).Build();

            _context.Roles.Add(role);
            _context.Users.Add(user);
            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetById(profile.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var responseDTO = Assert.IsType<GenericResponseDTO<ProfileEntity>>(okResult.Value);

            Assert.NotNull(responseDTO);
            Assert.True(responseDTO.Success);
            Assert.Equal(200, responseDTO.StatusCode);
            Assert.Equal("Elemento encontrado", responseDTO.Message);
        }

        ///  **Prueba: Crear perfil**
        [Fact]
        public async Task CreateProfile_ReturnsCreatedAtAction_WhenValid()
        {
            // Arrange
            var role = Role.Builder().SetName("Editor").SetDescription("Editor Role").Build();
            var user = User.Builder().WithName("David").WithEmail("david@example.com").WithRoles(new List<Role>{role}).Build();
            var profile = ProfileEntity.Builder().WithUser(user).WithRole(role).Build();

            _context.Roles.Add(role);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Create(profile);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            // Validación que la respuesta es un GenericResponseDTO<ProfileEntity>
            var responseDTO = Assert.IsType<GenericResponseDTO<ProfileEntity>>(createdAtResult.Value);

            Assert.NotNull(responseDTO);
            Assert.True(responseDTO.Success);
            Assert.Equal(201, responseDTO.StatusCode);
            Assert.Equal("Elemento creado correctamente", responseDTO.Message);
        }

        ///  **Prueba: Eliminar perfil**
        [Fact]
        public async Task DeleteProfile_ReturnsOk_WhenProfileExists()
        {
            // Arrange
            var role = Role.Builder().SetName("Tester").SetDescription("Tester Role").Build();
            var user = User.Builder().WithName("Eve").WithEmail("eve@example.com").WithRoles(new List<Role>{role}).Build();
            var profile = ProfileEntity.Builder().WithUser(user).WithRole(role).Build();

            _context.Roles.Add(role);
            _context.Users.Add(user);
            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Delete(profile.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<GenericResponseDTO<ProfileEntity>>(okResult.Value);

            Assert.Equal("Elemento eliminado correctamente", response.Message);
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equivalent(profile.Id, response.Data.Id);
            
        }
    }
}

