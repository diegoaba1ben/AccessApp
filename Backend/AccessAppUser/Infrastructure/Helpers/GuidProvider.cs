using System;

namespace AccessAppUser.Infrastructure.Helpers
{
    /// <summary>
    /// Proveedor de GUIDs para la aplicación.
    /// </summary>
    public static class GuidProvider
    {
        /// <summary>
        /// Genera un nuevo GUID.
        /// </summary>
        /// <returns>Un nuevo GUID.</returns>
        public static Guid NewGuid() => Guid.NewGuid();
    }
}