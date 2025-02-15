using System;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.DTOs.GesPass;
using AccessAppUser.Infrastructure.Persistence;

namespace AccessAppUser.Tests.DTOsTest.Unitarias
{
    public class GesPassDTOTest
    {
        [Fact]
        public void GesPassCreateDTO_Should_InitializeWithDefaultValues()
        {
            // Act
            var dto = new GesPassCreateDTO();

            // Assert
            dto.Should().NotBeNull();
            dto.UserId.Should().Be(Guid.Empty);
            dto.ResetToken.Should().BeEmpty();
            dto.TokenExpiration.Should().BeAfter(DateTime.MinValue);
        }

        [Fact]
        public void GesPassReadDTO_Should_InitializeWithDefaultValues()
        {
            // Act
            var dto = new GesPassReadDTO();

            // Assert
            dto.Should().NotBeNull();
            dto.Id.Should().Be(Guid.Empty);
            dto.UserId.Should().Be(Guid.Empty);
            dto.ResetToken.Should().BeEmpty();
            dto.TokenExpiration.Should().BeAfter(DateTime.MinValue);
            dto.IsCompleted.Should().BeFalse();
            dto.CompletedAt.Should().BeNull();
        }
    }
}