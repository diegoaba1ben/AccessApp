using System;
using System.Collections.Generic;
using AccessAppUser.Domain.Builders;

namespace AccessAppUser.Domain.Entities
{
    /// <summary>
    /// Representa un rol dentro del sistema, el cual puede tener múltiples permisos asociados.
    /// </summary>
    public class Role
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;

        /// <summary>
        /// Lista de permisos asociados a este rol.
        /// </summary>
        public List<Permission> Permissions { get; private set; } = new();

        /// <summary>
        /// Lista de áreas asociados a este rol.
        /// </summary>
        public List<Area> Areas { get; private set; } = new();

        /// <summary>
        /// Lista de usuarios asociados a este rol.
        /// </summary>
        public List<User> Users { get; private set; } = new();

        /// <summary>
        /// Lista intermedia para gestionar la relación con Permisos
        /// </summary>
        public List<RolePermission> RolePermissions { get; private set; } = new();

        /// <summary>
        /// Constructor privado para restringir la creación directa de instancias.
        /// </summary>
        private Role() { }

        /// <summary>
        /// Inicia la construcción de un nuevo objeto <see cref="Role"/>.
        /// </summary>
        /// <returns>Instancia de <see cref="RoleBuilder"/> para construir el rol.</returns>
        public static RoleBuilder Builder() => new RoleBuilder();

        /// <summary>
        /// Clase interna que implementa el patrón Builder para la entidad <see cref="Role"/>.
        /// </summary>
        public class RoleBuilder
        {
            private readonly Role _role;

            /// <summary>
            /// Constructor del builder, inicializa el rol con valores predeterminados.
            /// </summary>
            public RoleBuilder()
            {
                _role = new Role
                {
                    Id = Guid.NewGuid(),
                    Permissions = new List<Permission>()
                };
            }

            public RoleBuilder SetName(string name)
            {
                _role.Name = name;
                return this;
            }

            public RoleBuilder SetDescription(string description)
            {
                _role.Description = description;
                return this;
            }

            public RoleBuilder AddPermission(Permission permission)
            {
                _role.Permissions.Add(permission);
                return this;
            }

            public RoleBuilder AddPermissions(IEnumerable<Permission> permissions)
            {
                _role.Permissions.AddRange(permissions);
                return this;
            }

            /// <summary>
            /// Agrega una relación específica con un permiso utilizando la clase intermedia.
            /// </summary>
            public RoleBuilder AddRolePermission(Permission permission)
            {
                var rolePermission = new RolePermissionBuilder()
                    .WithRole(_role)
                    .WithPermission(permission)
                    .Build();

                _role.RolePermissions.Add(rolePermission);
                return this;
            }

            public RoleBuilder AddAreas(IEnumerable<Area> areas)
            {
                _role.Areas.AddRange(areas);
                return this;
            }

            public RoleBuilder AddUsers(IEnumerable<User> users)
            {
                _role.Users.AddRange(users);
                return this;
            }

            public Role Build() => _role;
        }
    }
}