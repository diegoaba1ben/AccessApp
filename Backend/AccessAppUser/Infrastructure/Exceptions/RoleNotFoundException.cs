using System;

namespace AccessAppUser.Infrastructure.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando no se encuentra un rol con el identificador proporcionado.
    /// </summary>
    public class RoleNotFoundException : AppException
    {
        /// <summary>
        /// Identificador del rol que no pudo ser localizado.
        /// </summary>
        public Guid RoleId { get; }

        /// <summary>
        /// Constructor que permite especificar el identificador fallido.
        /// </summary>
        /// <param name="roleId">Id del rol no encontrado</param>
        public RoleNotFoundException(Guid roleId)
            : base($"No se encontró ningún rol con el ID '{roleId}'.")
        {
            RoleId = roleId;
        }
    }
}