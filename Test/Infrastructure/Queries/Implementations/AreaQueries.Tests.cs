using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Persistence;
using AccessAppUser.Infrastructure.Queries.Implementations;

namespace AccessAppUser.Tests.Infrastructure.Queries
{
    public class AreaQueriesTests
    {
        private readonly AppDbContext _context;
        private readonly AreaQueries _areaQueries;

        public AreaQueriesTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestAreaDatabase")
                .Options;

            _context = new AppDbContext(options);
            _areaQueries = new AreaQueries(_context);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var area1 = Area.Builder().SetName("IT").SetDescription("Área de Tecnología").Build();
            var area2 = Area.Builder().SetName("HR").SetDescription("Área de Recursos Humanos").Build();

            var role1 = Role.Builder().SetName("Admin").SetDescription("Administrador del sistema").Build();
            var role2 = Role.Builder().SetName("User").SetDescription("Usuario estándar").Build();

            var profile = Profile.Builder().WithRole(role1).Build();

            area1.Roles.Add(role1);
            area2.Roles.Add(role2);

            var areaProfile = AreaProfile.Builder().WithArea(area1).WithProfile(profile).Build();
            area1.AreaProfiles.Add(areaProfile);

            _context.Areas.AddRange(area1, area2);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllAreasAsync_ShouldReturnAllAreas()
        {
            var result = await _areaQueries.GetAllAreasAsync();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAllAreasWithRolesAsync_ShouldReturnAreasWithRoles()
        {
            var result = await _areaQueries.GetAllAreasWithRolesAsync();
            result.Should().NotBeEmpty();
            result.All(a => a.Roles.Count > 0).Should().BeTrue();
        }

        [Fact]
        public async Task GetAllAreasWithProfilesAsync_ShouldReturnAreasWithProfiles()
        {
            var result = await _areaQueries.GetAllAreasWithProfilesAsync();
            result.Should().NotBeEmpty();
            result.All(a => a.AreaProfiles.Count > 0).Should().BeTrue();
        }

        [Fact]
        public async Task GetAreasByRoleNameAsync_ShouldReturnCorrectAreas()
        {
            var result = await _areaQueries.GetAreasByRoleNameAsync("Admin");
            result.Should().NotBeEmpty();
            result.All(a => a.Roles.Any(r => r.Name == "Admin")).Should().BeTrue();
        }

        [Fact]
        public async Task GetAreasByProfileIdAsync_ShouldReturnCorrectAreas()
        {
            var profileId = _context.Profiles.First().Id;
            var result = await _areaQueries.GetAreasByProfileIdAsync(profileId);
            result.Should().NotBeEmpty();
            result.All(a => a.AreaProfiles.Any(ap => ap.ProfileId == profileId)).Should().BeTrue();
        }
    }
}
