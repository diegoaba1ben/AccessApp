using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.DTOs.Profile;
using AccessAppUser.Infrastructure.Persistence;

namespace AccessAppUser.Tests.DTOsTest.Integracion
{
    public class ProfileDTOIntegrationTest
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_Profile")
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task ProfileDTO_Should_SaveAndRetrieveWithUserAndAreas()
        {
            // Arrange
            using var context = GetDbContext();

            var user = new User { Id = Guid.NewGuid(), Name = "Test User", Email = "testuser@example.com", Password = "SecurePass123!" };
            var area1 = new Area { Id = Guid.NewGuid(), Name = "Finance", Description = "Finance department" };
            var area2 = new Area { Id = Guid.NewGuid(), Name = "IT", Description = "IT department" };

            var profile = new Profile
            {
                Id = Guid.NewGuid(),
                User = user,
                UserName = user.Name,
                AreaProfiles = new List<AreaProfile>
                {
                    new AreaProfile { Area = area1 },
                    new AreaProfile { Area = area2 }
                }
            };

            // Act
            context.Users.Add(user);
            context.Areas.AddRange(area1, area2);
            context.Profiles.Add(profile);
            await context.SaveChangesAsync();

            var retrievedProfile = await context.Profiles
                .Include(p => p.User)
                .Include(p => p.AreaProfiles)
                .ThenInclude(ap => ap.Area)
                .FirstOrDefaultAsync(p => p.Id == profile.Id);

            // Assert
            retrievedProfile.Should().NotBeNull();
            retrievedProfile!.Id.Should().Be(profile.Id);
            retrievedProfile.User.Should().NotBeNull();
            retrievedProfile.User!.Name.Should().Be(user.Name);
            
            retrievedProfile.AreaProfiles.Should().NotBeNull();
            retrievedProfile.AreaProfiles.Should().HaveCount(2);
            retrievedProfile.AreaProfiles.Should().Contain(ap => ap.Area.Name == "Finance");
            retrievedProfile.AreaProfiles.Should().Contain(ap => ap.Area.Name == "IT");
        }
    }
}