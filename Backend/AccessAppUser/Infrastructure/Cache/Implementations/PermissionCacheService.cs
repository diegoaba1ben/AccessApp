using System.Text.Json;
using StackExchange.Redis;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Cache.Interfaces;

namespace AccessAppUser.Infrastructure.Cache.Implementations
{
    /// <summary>
    /// Implementación de la interfaz de caché para permisos
    /// </summary>
    public class PermissionCacheService : IPermissionCacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public PermissionCacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _database = redis.GetDatabase();
        }

        /// <summary>
        /// Obtiene un permiso de la caché por su ID
        /// </summary>
        /// <param name="id">ID del permiso</param>
        /// <returns>Permiso</returns>
        public async Task<Permission?> GetPermissionAsync(Guid id)
        {
            var value = await _database.StringGetAsync($"permission:{id}");
            if (value.IsNullOrEmpty)
            {
                return null;
            }
            if (value.HasValue && !string.IsNullOrEmpty(value)) // chequeo de nulidad
            {
                return JsonSerializer.Deserialize<Permission>(value!);
            }
            return null;
        }

        /// <summary>
        /// Establece el permiso para almacenar en la caché
        /// </summary>
        /// <param name="permission">Permiso a almacenar en la caché</param>
        public async Task SetPermissionAsync(Permission permission)
        {
            var serializedPermission = JsonSerializer.Serialize(permission);
            await _database.StringSetAsync($"permission:{permission.Id}", serializedPermission);
        }

        /// <summary>
        /// Elimina un permiso de la caché por su ID
        /// </summary>
        /// <param name="id">ID del permiso a eliminar</param>
        public async Task RemovePermissionAsync(Guid id)
        {
            await _database.KeyDeleteAsync($"permission:{id}");
        }

        /// <summary>
        /// Obtiene permisos de la caché por ID de rol
        /// </summary>
        /// <param name="roleId">ID del rol</param>
        public async Task<IEnumerable<Permission>?> GetPermissionsByRoleIdAsync(Guid roleId)
        {
            var value = await _database.StringGetAsync($"permissions:role:{roleId}");
            if (value.IsNullOrEmpty)
            {
                return null;
            }
            if (value.HasValue && !string.IsNullOrEmpty(value)) // chequeo de nulidad
            {
                var permission = JsonSerializer.Deserialize<Permission>(value!);
                if (permission != null)
                {
                    return new List<Permission> { permission };
                }
                return null;
            }
            return null;
        }

        /// <summary>
        /// Establece permisos en la caché por ID de rol
        /// </summary>
        /// <param name="roleId">ID del rol</param>
        /// <param name="permissions">Lista de permisos a almacenar en la caché</param>
        public async Task SetPermissionsByRoleIdAsync(Guid roleId, IEnumerable<Permission> permissions)
        {
            var serializedPermissions = JsonSerializer.Serialize(permissions);
            await _database.StringSetAsync($"permissions:role:{roleId}", serializedPermissions);
        }

        /// <summary>
        /// Elimina permisos de la caché por ID de rol
        /// </summary>
        /// <param name="roleId">ID del rol</param>
        public async Task RemovePermissionsByRoleIdAsync(Guid roleId)
        {
            await _database.KeyDeleteAsync($"permissions:role:{roleId}");
        }
    }
}