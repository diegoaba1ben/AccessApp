using System;
using Xunit;
using FluentAssertions;
using AccessAppUser.Infrastructure.DTOs;

namespace AccessAppUser.Tests.DTOsTest.Unitarias
{
    public class GenericResponseDTOTest
    {
        [Fact]
        public void GenericResponseDTO_Should_InitializeCorrectly()
        {
            // Arrange & Act
            var response = new GenericResponseDTO<string>(true, 200, "Operación exitosa", "Test Data");

            // Assert
            response.Should().NotBeNull();
            response.Success.Should().BeTrue();
            response.StatusCode.Should().Be(200);
            response.Message.Should().Be("Operación exitosa");
            response.Data.Should().Be("Test Data");
        }

        [Fact]
        public void GenericResponseDTO_SuccessResponse_ShouldReturnExpectedValues()
        {
            // Arrange & Act
            var response = GenericResponseDTO<string>.SuccessResponse("Test Data");

            // Assert
            response.Should().NotBeNull();
            response.Success.Should().BeTrue();
            response.StatusCode.Should().Be(200);
            response.Message.Should().Be("Operación exitosa");
            response.Data.Should().Be("Test Data");
        }

        [Fact]
        public void GenericResponseDTO_ErrorResponse_ShouldReturnExpectedValues()
        {
            // Arrange & Act
            var response = GenericResponseDTO<string>.ErrorResponse(404, "No encontrado");

            // Assert
            response.Should().NotBeNull();
            response.Success.Should().BeFalse();
            response.StatusCode.Should().Be(404);
            response.Message.Should().Be("No encontrado");
            response.Data.Should().BeNull();
        }
    }
}
