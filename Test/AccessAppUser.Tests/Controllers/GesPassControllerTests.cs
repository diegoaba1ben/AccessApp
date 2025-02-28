using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using AccessAppUser.Application.Controllers;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.DTOs.GesPass;
using AccessAppUser.Infrastructure.Persistence;
using AccessAppUser.Infrastructure.Repositories.Implementations;
using System;
using System.Threading.Tasks;

namespace AccessAppUser.Tests.Controllers
{
    public class GesPassControllerInMemoryTests
    {
        private readonly AppDbContext _context;
        private readonly GesPassRepository _gesPassRepository;
        private readonly UserRepository _userRepository;
        private readonly GesPassController _controller;

        public GesPassControllerInMemoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(options);
            _gesPassRepository = new GesPassRepository(_context);
            _userRepository = new UserRepository(_context);
            _controller = new GesPassController(_gesPassRepository, _userRepository);
        }

        [Fact]
        public async Task RequestPasswordReset_ReturnsOk_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Email = "test@example.com" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var request = new PasswordResetRequestDTO { Email = "test@example.com" };

            // Act
            var result = await _controller.RequestPasswordReset(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value); // Asegura que Value no sea null
            Assert.Contains("Token generado", okResult.Value?.ToString() ?? string.Empty);

        }

        [Fact]
        public async Task RequestPasswordReset_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new PasswordResetRequestDTO { Email = "nonexistent@example.com" };

            // Act
            var result = await _controller.RequestPasswordReset(request);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Usuario no encontrado", notFoundResult.Value);
        }

        [Fact]
        public async Task ValidateResetToken_ReturnsOk_WhenTokenIsValid()
        {
            // PASO 1: Limpiar la BD en memoria
            _context.Users.RemoveRange(_context.Users);
            _context.GesPasses.RemoveRange(_context.GesPasses);
            await _context.SaveChangesAsync();

            // PASO 2: Insertar un usuario 
            var userId = Guid.NewGuid();
            var testUser = new User
            {
                Id = userId,
                Email = "test@example.com",
                Password = "GesP@ss123"
            };

            // PASO 3: Crear GesPass con token
            var testGesPass = new GesPass
            {
                UserId = userId,
                ResetToken = "vGesP@ss123",
                TokenExpiration = DateTime.UtcNow.AddMinutes(10)
            };

            // PASO 4: Insertar registros
            _context.Users.Add(testUser);
            _context.GesPasses.Add(testGesPass);
            await _context.SaveChangesAsync();

            // PASO 5: Verificar existencia
            var checkUser = await _context.Users.FindAsync(testUser.Id);
            var checkGesPass = await _gesPassRepository.GetByResetTokenAsync("vGesP@ss123"); //  Busca por token
            Assert.NotNull(checkUser);
            Assert.NotNull(checkGesPass);

            // PASO 6: Crear request
            var request = new TokenValidationDTO { Token = "vGesP@ss123" }; // Faltaba

            // Act
            var result = await _controller.ValidateResetToken(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Token válido", okResult.Value);
        }

        [Fact]
        public async Task ValidateResetToken_ReturnsBadRequest_WhenTokenIsInvalid()
        {
            // Arrange
            var request = new TokenValidationDTO { Token = "invalidtoken" };

            // Act
            var result = await _controller.ValidateResetToken(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Token inválido o expirado", badRequestResult.Value);
        }

        [Fact]
        public async Task ResetPassword_ReturnsOk_WhenTokenIsValid()
        {
            // Arrange
            var gesPass = new GesPass
            {
                ResetToken = "validtoken",
                TokenExpiration = DateTime.UtcNow.AddHours(1),
                UserId = Guid.NewGuid()
            };

            var user = new User
            {
                Id = gesPass.UserId,
                Email = "test@example.com",
                Password = "oldpassword"
            };

            _context.GesPasses.Add(gesPass);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // PASO 1: Verificar que los usuarios se creen en BD
            Console.WriteLine($" Usuario insertado: {user.Id} - {user.Email}");
            Console.WriteLine($" GesPass insertado: {gesPass.UserId} - Token {gesPass.ResetToken}");

            var request = new PasswordResetDTO { Token = "validtoken", NewPassword = "newpassword" };

            // Act
            var result = await _controller.ResetPassword(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Contraseña restablecida correctamente", okResult.Value);
        }

        [Fact]
        public async Task ResetPassword_ReturnsBadRequest_WhenTokenIsInvalid()
        {
            // Arrange
            var request = new PasswordResetDTO { Token = "invalidtoken", NewPassword = "newpassword" };

            // Act
            var result = await _controller.ResetPassword(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Token inválido o expirado", badRequestResult.Value);
        }
    }
}