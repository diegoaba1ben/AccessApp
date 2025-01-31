using System;

namespace AccessAppUser.Infrastructure.Helpers
{
    /// <summary>
    /// Proveedor de GUIDs para la aplicaci�n.
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