using System;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.DTOs.User;
using AccessAppUser.Infrastructure.Persistence;

namespace AccessAppUser.Tests.DTOsTest.Unitarias
{
    public class UserDTOTest
    {
        [Fact]
        public void UserCreateDTO_Should_InitializeWithDefaultValues()
        {
            // Act
            var dto = new UserCreateDTO();

            // Assert
            dto.Should().NotBeNull();
            dto.Name.Should().BeEmpty();
            dto.Email.Should().BeEmpty();
            dto.Password.Should().BeEmpty();
            dto.IsActive.Should().BeTrue();
        }

        [Fact]
        public void UserReadDTO_Should_InitializeWithDefaultValues()
        {
            // Act
            var dto = new UserReadDTO();

            // Assert
            dto.Should().NotBeNull();
            dto.Id.Should().Be(Guid.Empty);
            dto.Name.Should().BeEmpty();
            dto.Email.Should().BeEmpty();
            dto.IsActive.Should().BeFalse();
        }

        [Fact]
        public void UserUpdateDTO_Should_InitializeWithDefaultValues()
        {
            // Act
            var dto = new UserUpdateDTO();

            // Assert
            dto.Should().NotBeNull();
            dto.Id.Should().Be(Guid.Empty);
            dto.Name.Should().BeEmpty();
            dto.Email.Should().BeEmpty();
            dto.Password.Should().BeEmpty();
            dto.IsActive.Should().BeFalse();
        }
    }
}
