using System;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.DTOs.Area;
using AccessAppUser.Infrastructure.Persistence;

namespace AccessAppUser.Tests.DTOsTest.Unitarias
{
    public class AreaDTOTest
    {
        [Fact]
        public void AreaCreateDTO_Should_InitializeWithDefaultValues()
        {
            // Act
            var dto = new AreaCreateDTO();

            // Assert
            dto.Should().NotBeNull();
            dto.Name.Should().BeEmpty();
            dto.Description.Should().BeEmpty();
            dto.AssociatedRolesIds.Should().NotBeNull();
            dto.AssociatedRolesIds.Should().BeEmpty();
        }

        [Fact]
        public void AreaReadDTO_Should_InitializeWithDefaultValues()
        {
            // Act
            var dto = new AreaReadDTO();

            // Assert
            dto.Should().NotBeNull();
            dto.Id.Should().Be(Guid.Empty);
            dto.Name.Should().BeEmpty();
            dto.Description.Should().BeEmpty();
            dto.AssociatedRoles.Should().NotBeNull();
            dto.AssociatedRoles.Should().BeEmpty();
        }
    }
}
