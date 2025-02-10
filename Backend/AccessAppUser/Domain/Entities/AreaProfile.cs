using System;

namespace AccessAppUser.Domain.Entities
{
    /// <summary>
    /// Tabla intermedia para relacionar áreas y perfiles.
    /// </summary>
    public class AreaProfile
    {
        public Guid AreaId { get; private set; }
        public Guid ProfileId { get; private set; }

        public Area Area { get; private set; } = null!;
        public Profile Profile { get; private set; } = null!;

        /// <summary>
        /// Constructor privado para evitar instanciación directa.
        /// </summary>
        private AreaProfile() { }

        /// <summary>
        /// Inicializa el builder interno.
        /// </summary>
        public static AreaProfileBuilder Builder() => new AreaProfileBuilder();

        /// <summary>
        /// Builder interno para la creación de AreaProfile.
        /// </summary>
        public class AreaProfileBuilder
        {
            private readonly AreaProfile _areaProfile;

            /// <summary>
            /// Constructor del Builder, inicializa la entidad.
            /// </summary>
            public AreaProfileBuilder()
            {
                _areaProfile = new AreaProfile();
            }

            /// <summary>
            /// Establece el área asociada al perfil.
            /// </summary>
            public AreaProfileBuilder WithArea(Area area)
            {
                if (area == null) throw new ArgumentNullException(nameof(area), "El área no puede ser nula.");
                _areaProfile.Area = area;
                _areaProfile.AreaId = area.Id;
                return this;
            }

            /// <summary>
            /// Establece el perfil asociado al área.
            /// </summary>
            public AreaProfileBuilder WithProfile(Profile profile)
            {
                if (profile == null) throw new ArgumentNullException(nameof(profile), "El perfil no puede ser nulo.");
                _areaProfile.Profile = profile;
                _areaProfile.ProfileId = profile.Id;
                return this;
            }

            /// <summary>
            /// Finaliza la construcción del objeto <see cref="AreaProfile"/>.
            /// </summary>
            public AreaProfile Build() => _areaProfile;
        }
    }
}