using System;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.DTOs.Profile;
using AccessAppUser.Infrastructure.Persistence;

namespace AccessAppUser.Tests.DTOsTest.Unitarias
{
    public class ProfileDTOTest
    {
        [Fact]
        public void ProfileCreateDTO_Should_InitializeWithDefaultValues()
        {
            // Act
            var dto = new ProfileCreateDTO();

            // Assert
            dto.Should().NotBeNull();
            dto.UserId.Should().Be(Guid.Empty);
            dto.RoleId.Should().Be(Guid.Empty);
            dto.AssociatedAreaIds.Should().NotBeNull();
            dto.AssociatedAreaIds.Should().BeEmpty();
        }

        [Fact]
        public void ProfileReadDTO_Should_InitializeWithDefaultValues()
        {
            // Act
            var dto = new ProfileReadDTO();

            // Assert
            dto.Should().NotBeNull();
            dto.Id.Should().Be(Guid.Empty);
            dto.UserName.Should().BeEmpty();
            dto.RoleName.Should().BeEmpty();
            dto.AssociatedAreas.Should().NotBeNull();
            dto.AssociatedAreas.Should().BeEmpty();
        }
    }
}