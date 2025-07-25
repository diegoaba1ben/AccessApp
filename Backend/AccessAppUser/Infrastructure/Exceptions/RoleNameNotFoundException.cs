using System;

namespace AccessAppUser.Infrastructure.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando no se encuentra un rol con el nombre especificado.
    /// </summary>
    public class RoleNameNotFoundException : AppException
    {
        /// <summary>
        /// Nombre del rol que no pudo ser localizado.
        /// </summary>
        public string RoleName { get; }

        /// <summary>
        /// Constructor que permite especificar el nombre fallido.
        /// </summary>
        /// <param name="roleName">Nombre del rol no encontrado</param>
        public RoleNameNotFoundException(string roleName)
            : base($"No se encontró ningún rol con el nombre '{roleName}'.")
        {
            RoleName = roleName;
        }
    }
}