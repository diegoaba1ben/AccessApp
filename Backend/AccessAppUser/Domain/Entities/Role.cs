using System;
using System.Collections.Generic;

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
                    Permissions = new List<Permission>() // Asegura que la lista nunca sea nula
                };
            }

            /// <summary>
            /// Establece el nombre del rol.
            /// </summary>
            /// <param name="name">Nombre del rol.</param>
            /// <returns>Instancia de <see cref="RoleBuilder"/> para encadenamiento.</returns>
            public RoleBuilder SetName(string name)
            {
                _role.Name = name;
                return this;
            }

            /// <summary>
            /// Establece la descripción del rol.
            /// </summary>
            /// <param name="description">Descripción del rol.</param>
            /// <returns>Instancia de <see cref="RoleBuilder"/> para encadenamiento.</returns>
            public RoleBuilder SetDescription(string description)
            {
                _role.Description = description;
                return this;
            }

            /// <summary>
            /// Agrega un permiso a la lista de permisos del rol.
            /// </summary>
            /// <param name="permission">Permiso a agregar.</param>
            /// <returns>Instancia de <see cref="RoleBuilder"/> para encadenamiento.</returns>
            public RoleBuilder AddPermission(Permission permission)
            {
                _role.Permissions.Add(permission);
                return this;
            }

            /// <summary>
            /// Agrega una colección de permisos a la lista de permisos del rol.
            /// </summary>
            /// <param name="permissions">Lista de permisos a agregar.</param>
            /// <returns>Instancia de <see cref="RoleBuilder"/> para encadenamiento.</returns>
            public RoleBuilder AddPermissions(IEnumerable<Permission> permissions)
            {
                _role.Permissions.AddRange(permissions);
                return this;
            }

            /// <summary>
            /// Agrega una colección de áreas a la lista de áreas asociadas al rol.
            /// </summary>
            /// <param name="areas">Lista de permisos a agregar.</param>
            /// <returns>Instancia de <see cref="RoleBuilder"/> para encadenamiento.</returns>
            public RoleBuilder AddAreas(IEnumerable<Area> areas)
            {
                _role.Areas.AddRange(areas);
                return this;
            }

            /// <summary>
            /// Agrega una colección de usuarios a la lista de usuarios asociados al rol.
            /// </summary>
            /// <param name="users">Lista de permisos a agregar.</param>
            /// <returns>Instancia de <see cref="RoleBuilder"/> para encadenamiento.</returns>
            public RoleBuilder AddUsers(IEnumerable<User> users)
            {
                _role.Users.AddRange(users);
                return this;
            }

            /// <summary>
            /// Finaliza la construcción del objeto <see cref="Role"/>.
            /// </summary>
            /// <returns>Instancia construida de <see cref="Role"/>.</returns>
            public Role Build() => _role;
        }
    }
}
