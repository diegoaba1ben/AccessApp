using System;

namespace AccessAppUser.Infrastructure.Helpers
{
    /// <summary>
    /// Utilidad para generar claves consistentes para elmacenamiento en caché 
    /// Formatea las claves utilizando el tipo de entidad y un identificador único.
    /// </summary>
    public static class CacheKeyFormatter
    {
        ///<summary>
        /// Genera una clave única en el formato "tipo:nombreguid" utilizando el nombre de la entidad y su identificador
        /// ejemplo: "role:71dd61ce1dd147dcbb765c5b6e7fd355"
        /// </summary>
        /// <typeparma name="T"> Tipo de entidad que se representa en la clave (ej: Role, User, etc.)</typeparma>
        /// <param name="id">Identificador único de la entidad, <see cref="Guid"> de la entidad</param>
        /// <returns>Clave formateada como string</returns>
        public static string GetKey<T>(Guid id)
        { 
            var typeName = typeof(T).Name.ToLowerInvariant();
            return $"{typeName}:{id.ToString("N")}";
        }
    }
}