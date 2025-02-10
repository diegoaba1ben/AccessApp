using System;
using AccessAppUser.Infrastructure.Helpers;

namespace AccessAppUser.Domain.Entities
{
    /// <summary>
    /// Representa un permiso dentro del sistema.
    /// </summary>
    public class Permission
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;

        // Constructor privado para forzar el uso del builder
        private Permission() { }

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
                _permission = new Permission { Id = GuidProvider.NewGuid() }; // 🔥 Aquí se usa GuidProvider
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

            public Permission Build() => _permission;
        }
    }
}
