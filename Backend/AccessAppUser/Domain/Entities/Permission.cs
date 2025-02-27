using AccessAppUser.Domain.Builders;
using AccessAppUser.Infrastructure.Helpers;

namespace AccessAppUser.Domain.Entities
{
    /// <summary>
    /// Representa un permiso dentro del sistema.
    /// </summary>
    public class Permission
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Lista de roles asociados a este permiso.
        /// </summary>
        public List<Role> Roles { get; set; } = new();

        /// <summary>
        /// Lista intermedia para gestionar la relación con Roles.
        /// </summary>
        public List<RolePermission> RolePermissions {get; private set; } = new();

        // Constructor vacío para forzar el uso del builder
        public Permission() { }

        /// <summary>
        /// Método estático para inicializar el Builder.
        /// </summary>
        public static PermissionBuilder Builder() => new PermissionBuilder();

        /// <summary>
        /// Builder interno para construir instancias de Permission.
        /// </summary>
        public class PermissionBuilder
        {
            private readonly Permission _permission;

            public PermissionBuilder()
            {
                _permission = new Permission { Id = GuidProvider.NewGuid() };
            }

            public PermissionBuilder WithName(string name)
            {
                _permission.Name = name.Trim();
                return this;
            }

            public PermissionBuilder WithDescription(string description)
            {
                _permission.Description = description.Trim();
                return this;
            }

            public PermissionBuilder WithRoles(IEnumerable<Role> roles)
            {
                _permission.Roles = new List<Role>(roles);
                return this;
            }

            /// <summary>
            /// Tabla intermedia para gestionar la relación con rol
            /// </summary>
            public PermissionBuilder AddRole(Role role)
            {
                var rolePermission = new RolePermissionBuilder()
                    .WithRole(role)
                    .WithPermission(_permission)
                    .Build();

                _permission.RolePermissions.Add(rolePermission);
                return this;
            }

            public Permission Build() => _permission;
        }
    }
}
