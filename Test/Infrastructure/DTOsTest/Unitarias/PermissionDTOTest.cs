using System;
using System.Collection.Generic;
using System.Linq;
using System.Threading.Task;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Persistence;

namespace AccessAppUser.Test.Infrastructure.DTOsTest.Unitarias
{
    public class PermissionDTOTest
    {
        [Fact]
        public void PermissionCreateDTO_Should_Initialized_With_Default_Values()
        {
            // Act
            var dto = new PermissionCreateDTO();

            // Assert
            dto.sholud().NotBeNull();
            dto.Name.Should().BeEmpty();
            dto.Description.Should().BeEmpty;
        }
        [Fact]
        public void PermissionReadDTO_Should_Initialize_With_Default_Values()
        {
            // Act
            var dto = new PermissionReadDTO()

            // Assert
            dto.Should().NotBeNull();
            dto.Id.Should().Be(Guid.Empty);
            dto.Name.Should().BeEmpty();
            dto.Description.Should().BeEmpty();
        }
    }
}