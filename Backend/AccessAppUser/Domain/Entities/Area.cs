using System;
using System.Collections.Generic;

namespace AccessAppUser.Domain.Entities
{
    /// <summary>
    /// Representa un área funcional dentro del sistema.
    /// </summary>
    public class Area
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;

        public List<Role> Roles { get; private set; } = new();
        public List<AreaProfile> AreaProfiles { get; private set; } = new();

        /// <summary>
        /// Constructor privado para restringir la creación directa de instancias.
        /// </summary>
        private Area() { }

        /// <summary>
        /// Inicia la construcción de un nuevo objeto <see cref="Area"/>.
        /// </summary>
        /// <returns>Instancia de <see cref="AreaBuilder"/> para construir el área.</returns>
        public static AreaBuilder Builder() => new AreaBuilder();

        /// <summary>
        /// Clase interna que implementa el patrón Builder para la entidad <see cref="Area"/>.
        /// </summary>
        public class AreaBuilder
        {
            private readonly Area _area;

            /// <summary>
            /// Constructor del builder, inicializa el área con valores predeterminados.
            /// </summary>
            public AreaBuilder()
            {
                _area = new Area
                {
                    Id = Guid.NewGuid(),
                    Roles = new List<Role>(), // Asegura que la lista nunca sea nula
                    AreaProfiles = new List<AreaProfile>()
                };
            }

            /// <summary>
            /// Establece el nombre del área.
            /// </summary>
            public AreaBuilder SetName(string name)
            {
                _area.Name = name;
                return this;
            }

            /// <summary>
            /// Establece la descripción del área.
            /// </summary>
            public AreaBuilder SetDescription(string description)
            {
                _area.Description = description;
                return this;
            }

            /// <summary>
            /// Agrega un rol a la lista de roles del área.
            /// </summary>
            public AreaBuilder AddRole(Role role)
            {
                _area.Roles.Add(role);
                return this;
            }

            /// <summary>
            /// Agrega múltiples roles a la lista de roles del área.
            /// </summary>
            public AreaBuilder AddRoles(IEnumerable<Role> roles)
            {
                _area.Roles = roles?.ToList() ?? new List<Role>(); // Permite listas vacías
                return this;
            }

            /// <summary>
            /// Asocia un perfil al área mediante la tabla intermedia <see cref="AreaProfile"/>.
            /// </summary>
            public AreaBuilder AddProfile(Profile profile)
            {
                _area.AreaProfiles.Add(
                    AreaProfile.Builder() // Usamos el Builder en lugar de instanciarlo directamente.
                        .WithArea(_area)
                        .WithProfile(profile)
                        .Build()
                );
                return this;
            }

            /// <summary>
            /// Finaliza la construcción del objeto <see cref="Area"/>.
            /// </summary>
            public Area Build() => _area;
        }
    }
}
