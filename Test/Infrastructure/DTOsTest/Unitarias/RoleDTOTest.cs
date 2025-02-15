using System;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.DTOs.Role;
using AccessAppUser.Infrastructure.Persistence;

namespace AccessAppUser.Tests.DTOsTest.Unitarias
{
    public class RoleDTOTest
    {
        [Fact]
        public void RoleCreateDTO_Should_InitializeWithDefaultValues()
        {
            // Act
            var dto = new RoleCreateDTO();

            // Assert
            dto.Should().NotBeNull();
            dto.Name.Should().BeEmpty();
            dto.Description.Should().BeEmpty();
            dto.PermissionIds.Should().NotBeNull();
            dto.PermissionIds.Should().BeEmpty();
        }

        [Fact]
        public void RoleReadDTO_Should_InitializeWithDefaultValues()
        {
            // Act
            var dto = new RoleReadDTO();

            // Assert
            dto.Should().NotBeNull();
            dto.Id.Should().Be(Guid.Empty);
            dto.Name.Should().BeEmpty();
            dto.Description.Should().BeEmpty();
            dto.Permissions.Should().NotBeNull();
            dto.Permissions.Should().BeEmpty();
        }
    }
}