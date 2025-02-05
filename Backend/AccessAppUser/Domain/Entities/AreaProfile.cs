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

        // Cambiar internal a public para que sea accesible desde otros lugares
        public Area Area { get; internal set; } = null!;
        public Profile Profile { get; internal set; } = null!;

        /// <summary>
        /// Constructor privado para evitar instanciación directa.
        /// </summary>
        public AreaProfile() { }

        /// <summary>
        /// Método para asignar un área a esta relación.
        /// </summary>
        public void SetArea(Area area)
        {
            if (area == null) throw new ArgumentNullException(nameof(area));
            Area = area;
            AreaId = area.Id;
        }

        /// <summary>
        /// Método para asignar un perfil a esta relación.
        /// </summary>
        public void SetProfile(Profile profile)
        {
            if (profile == null) throw new ArgumentNullException(nameof(profile));
            Profile = profile;
            ProfileId = profile.Id;
        }

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
                _areaProfile.SetArea(area);
                return this;
            }

            /// <summary>
            /// Establece el perfil asociado al área.
            /// </summary>
            public AreaProfileBuilder WithProfile(Profile profile)
            {
                _areaProfile.SetProfile(profile);
                return this;
            }

            public AreaProfile Build() => _areaProfile;
        }
    }
}


