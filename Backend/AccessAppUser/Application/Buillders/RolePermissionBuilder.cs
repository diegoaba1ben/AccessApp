using AccessAppUser.Domain.Entities;
using System;

namespace AccessAppUser.Domain.Builders
{
    public class RolePermissionBuilder
    {
        private readonly RolePermission _rolePermission;

        public RolePermissionBuilder()
        {
            _rolePermission = new RolePermission
            {
                AssignedAt = DateTime.UtcNow
            };
        }

        public RolePermissionBuilder WithRole(Role role)
        {
            _rolePermission.Role = role;
            _rolePermission.RoleId = role.Id;
            return this;
        }

        public RolePermissionBuilder WithPermission(Permission permission)
        {
            _rolePermission.Permission = permission;
            _rolePermission.PermissionId = permission.Id;
            return this;
        }

        public RolePermissionBuilder WithAssignmentDate(DateTime date)
        {
            _rolePermission.AssignedAt = date;
            return this;
        }

        public RolePermission Build()
        {
            return _rolePermission;
        }
    }
}
