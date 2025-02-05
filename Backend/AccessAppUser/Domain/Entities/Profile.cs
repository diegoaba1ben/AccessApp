using System;
using System.Collections.Generic;
using AccessAppUser.Infrastructure.Helpers; // Asegúrate de incluir esto si usas GuidProvider

namespace AccessAppUser.Domain.Entities
{
    /// <summary>
    /// Representa el perfil de un usuario dentro del sistema.
    /// </summary>
    public class Profile
    {
        public Guid Id { get; private set; } // Se recibe desde User
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        // Relaciones
        public User User { get; private set; } = null!;
        public List<AreaProfile> AreaProfiles { get; private set; } = new();
        public Role Role { get; private set; } = null!;

        /// <summary>
        /// Constructor privado para evitar instanciación directa.
        /// </summary>
        private Profile() { }

        /// <summary>
        /// Método estático para inicializar el Builder.
        /// </summary>
        public static ProfileBuilder Builder() => new ProfileBuilder();

        /// <summary>
        /// Builder interno para construir instancias de Profile.
        /// </summary>
        public class ProfileBuilder
        {
            private readonly Profile _profile;

            public ProfileBuilder()
            {
                _profile = new Profile();
            }

            public ProfileBuilder WithUser(User user)
            {
                _profile.User = user;
                _profile.Id = user.Id; // Aseguramos que el ID de Profile viene de User
                return this;
            }

            public ProfileBuilder WithRole(Role role)
            {
                _profile.Role = role;
                return this;
            }

            public ProfileBuilder WithAreaProfiles(List<AreaProfile> areaProfiles)
            {
                _profile.AreaProfiles = areaProfiles ?? new List<AreaProfile>();
                return this;
            }

            public Profile Build() => _profile;
        }
    }
}
