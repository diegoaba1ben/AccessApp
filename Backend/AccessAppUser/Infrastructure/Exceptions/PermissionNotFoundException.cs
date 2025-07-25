using System;

namespace AccessAppUser.Infrastructure.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando no se encuentra un permiso con el ID especificado.
    /// </summary>
    public class PermissionNotFoundException : AppException
    {
        public Guid PermissionId { get; }

        public PermissionNotFoundException(Guid permissionId)
            : base($"No se encontró ningún permiso con el ID '{permissionId}'.")
        {
            PermissionId = permissionId;
        }
    }
}