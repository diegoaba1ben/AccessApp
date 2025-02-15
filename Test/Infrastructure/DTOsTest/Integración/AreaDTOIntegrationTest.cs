using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.DTOs.Area;
using AccessAppUser.Infrastructure.Persistence;

namespace AccessAppUser.Tests.DTOsTest.Integracion
{
    public class AreaDTOIntegrationTest
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_Area")
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task AreaDTO_Should_SaveAndRetrieveCorrectly()
        {
            // Arrange
            using var context = GetDbContext();
            var area = new Area
            {
                Id = Guid.NewGuid(),
                Name = "Test Area",
                Description = "Area for testing purposes"
            };

            // Act
            context.Areas.Add(area);
            await context.SaveChangesAsync();

            var retrievedArea = await context.Areas.FirstOrDefaultAsync(a => a.Id == area.Id);

            // Assert
            retrievedArea.Should().NotBeNull();
            retrievedArea!.Id.Should().Be(area.Id);
            retrievedArea.Name.Should().Be(area.Name);
            retrievedArea.Description.Should().Be(area.Description);
        }
    }
}
