using System;

namespace AccessAppUser.Infrastructure.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando no se encuentra la relación entre un rol y un permiso.
    /// </summary>
    public class RolePermissionNotFoundException : AppException
    {
        public Guid RoleId { get; }
        public Guid PermissionId { get; }

        public RolePermissionNotFoundException(Guid roleId, Guid permissionId)
            : base($"No se encontró la relación entre el rol '{roleId}' y el permiso '{permissionId}'.")
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }
    }
}