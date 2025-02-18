using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using AccessAppUser.Application.Controllers;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Repositories.Interfaces;
using AccessAppUser.Infrastructure.DTOs;

namespace AccessAppUser.Tests.Controllers
{
    public class PermissionControllerTests
    {
        private readonly Mock<IPermissionRepository> _mockRepo;
        private readonly PermissionController _controller;

        public PermissionControllerTests()
        {
            _mockRepo = new Mock<IPermissionRepository>();
            _controller = new PermissionController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllPermissions_ReturnsOk_WithPermissions()
        {
            // Arrange
            var permissions = new List<Permission>
            {
                new Permission { Id = Guid.NewGuid(), Name = "Read", Description = "Read permission" },
                new Permission { Id = Guid.NewGuid(), Name = "Write", Description = "Write permission" }
            };

            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(permissions);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<GenericResponseDTO<IEnumerable<Permission>>>(okResult.Value);
            Assert.Equal(2, response.Data!.Count());
        }

        [Fact]
        public async Task GetPermissionById_ReturnsNotFound_WhenPermissionDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(default(Permission));

            // Act
            var result = await _controller.GetById(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreatePermission_ReturnsCreatedAtAction_WhenValid()
        {
            // Arrange
            var permission = new Permission { Id = Guid.NewGuid(), Name = "Execute", Description = "Execute permission" };

            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Permission>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(permission);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var response = Assert.IsType<GenericResponseDTO<Permission>>(createdAtResult.Value);
            Assert.Equal(permission.Name, response.Data!.Name);
        }
    }
}

