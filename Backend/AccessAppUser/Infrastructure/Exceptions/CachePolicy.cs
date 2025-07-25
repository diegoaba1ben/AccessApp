using System;

namespace AccessAppUser.Infrastructure.Configuration
{
    /// <summary>
    /// Define políticas técnicas para el uso de caché en la infraestructura.
    /// </summary>
    public static class CachePolicy
    {
        /// <summary>
        /// Tiempo de vida mínimo para valores sensibles en caché.
        /// </summary>
        public static TimeSpan SensitiveTtl => TimeSpan.FromSeconds(5);

        /// <summary>
        /// Tiempo de vida recomendado para valores estándar en caché.
        /// </summary>
        public static TimeSpan DefaultTtl => TimeSpan.FromMinutes(2);

        /// <summary>
        /// Tiempo de vida extendido para valores poco volátiles.
        /// </summary>
        public static TimeSpan LongTermTtl => TimeSpan.FromMinutes(15);
    }
}