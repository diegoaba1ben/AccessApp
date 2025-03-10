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
        public Guid Id { get; set; } // Se recibe desde User
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relaciones
        public User User { get; set; } = null!;
        public List<AreaProfile> AreaProfiles { get; set; } = new();
        public Role Role { get; set; } = null!;

        /// <summary>
        /// Constructor vacío para evitar instanciación directa.
        /// </summary>
        public Profile() { }

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
                if (user == null)
                    throw new ArgumentNullException(nameof(user), "Cada perfil debe estar asociado a un usuario.");

                _profile.User = user;
                _profile.Id = user.Id; // Aseguramos que el ID de Profile viene de User
                return this;
            }

            public ProfileBuilder WithRole(Role role)
            {   if(role == null)
                    throw new ArgumentNullException(nameof(role), "Cada perfil debe tener un rol asociado.");

                _profile.Role = role;
                return this;
            }

            public ProfileBuilder WithAreaProfiles(List<AreaProfile> areaProfiles)
            {
                if(areaProfiles == null || areaProfiles.Count == 0)
                    throw new ArgumentNullException(nameof(areaProfiles), "El perfil debe estr vinculado a por lo menos un área.");

                _profile.AreaProfiles = areaProfiles;
                return this;
            }

            public Profile Build() => _profile;
        }
    }
}