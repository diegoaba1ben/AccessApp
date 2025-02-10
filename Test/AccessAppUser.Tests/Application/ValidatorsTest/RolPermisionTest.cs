using Xunit;
using FluentAssertions;
using AccessAppUser.Domain.Entities;
using System.Collections.Generic;

namespace AccessAppUser.Tests.Application.Validators
{
    public class RolePermissionTests
    {
        [Fact]
        public void Role_Should_Allow_Adding_Permissions()
        {
            // Arrange
            var permission1 = Permission.Builder()
                .WithName("Read Reports")
                .WithDescription("Allows reading financial reports")
                .Build();

            var permission2 = Permission.Builder()
                .WithName("Edit Users")
                .WithDescription("Allows modifying user accounts")
                .Build();

            var role = Role.Builder()
                .SetName("Manager")
                .SetDescription("Manager role with elevated privileges")
                .AddPermissions(new List<Permission> { permission1, permission2 })
                .Build();

            // Act & Assert
            role.Permissions.Should().NotBeEmpty()
                .And.HaveCount(2)
                .And.Contain(permission1)
                .And.Contain(permission2);
        }

        [Fact]
        public void Role_Should_Allow_Empty_Permissions_List()
        {
            // Arrange
            var role = Role.Builder()
                .SetName("Intern")
                .SetDescription("Limited access role")
                .Build();

            // Act & Assert
            role.Permissions.Should().BeEmpty();
        }
    }
}
