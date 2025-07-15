using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Cache.Interfaces;
using AccessAppUser.Infrastructure.Helpers;

namespace AccessAppUser.Infrastructure.Cache.Implementations
{
    /// <summary>
    /// Servicio de caché especializado para entidades <see cref="Permission"/> individuales y agrupadas por rol.
    /// Implementa el patrón CME delegando en servicios base genéricos.
    /// </summary>
    public class PermissionCacheService : IPermissionCacheService
    {
        private readonly IBaseCacheService<Permission> _singlePermissionCache;
        private readonly IBaseCacheService<IEnumerable<Permission>> _permissionsByRoleCache;
        private readonly ILogger<PermissionCacheService> _logger;

        /// <summary>
        /// Inicializa una nueva instancia del servicio <see cref="PermissionCacheService"/>.
        /// </summary>
        /// <param name="singlePermissionCache">Servicio de caché para entidad <see cref="Permission"/>.</param>
        /// <param name="permissionsByRoleCache">Servicio de caché para colecciones de permisos.</param>
        /// <param name="logger">Logger para trazabilidad y manejo de errores.</param>
        public PermissionCacheService(
            IBaseCacheService<Permission> singlePermissionCache,
            IBaseCacheService<IEnumerable<Permission>> permissionsByRoleCache,
            ILogger<PermissionCacheService> logger)
        {
            _singlePermissionCache = singlePermissionCache;
            _permissionsByRoleCache = permissionsByRoleCache;
            _logger = logger;
        }

        public async Task<Permission?> GetPermissionAsync(Guid id)
        {
            var key = CacheKeyFormatter.GetKey<Permission>(id);
            try
            {
                var permission = await _singlePermissionCache.GetAsync(key);
                if (permission == null)
                {
                    _logger.LogInformation("Permiso con clave {Key} no encontrado en caché.", key);
                }
                return permission;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar permiso con clave: {Key}.", key);
                throw;
            }
        }

        public async Task SetPermissionAsync(Permission permission)
        {
            var key = CacheKeyFormatter.GetKey<Permission>(permission.Id);
            try
            {
                await _singlePermissionCache.SetAsync(key, permission, TimeSpan.FromMinutes(30));
                _logger.LogInformation("Permiso almacenado con clave {Key}.", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al almacenar permiso con clave: {Key}.", key);
                throw;
            }
        }

        public async Task RemovePermissionAsync(Guid id)
        {
            var key = CacheKeyFormatter.GetKey<Permission>(id);
            try
            {
                await _singlePermissionCache.RemoveAsync(key);
                _logger.LogInformation("Permiso eliminado de la caché con clave: {Key}.", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar permiso con clave: {Key}.", key);
                throw;
            }
        }

        public async Task<IEnumerable<Permission>?> GetPermissionsByRoleIdAsync(Guid roleId)
        {
            var key = $"role:{roleId.ToString("N")}:permissions";
            try
            {
                var permissions = await _permissionsByRoleCache.GetAsync(key);
                if (permissions == null)
                {
                    _logger.LogWarning("Permisos para el rol con clave {Key} no encontrados.", key);
                }
                return permissions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar permisos de rol con clave: {Key}.", key);
                throw;
            }
        }

        public async Task SetPermissionsByRoleIdAsync(Guid roleId, IEnumerable<Permission> permissions)
        {
            var key = $"role:{roleId.ToString("N")}:permissions";
            try
            {
                await _permissionsByRoleCache.SetAsync(key, permissions, TimeSpan.FromMinutes(30));
                _logger.LogInformation("Permisos para rol almacenados con clave: {Key}.", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al almacenar permisos de rol con clave: {Key}.", key);
                throw;
            }
        }

        public async Task RemovePermissionsByRoleIdAsync(Guid roleId)
        {
            var key = $"role:{roleId.ToString("N")}:permissions";
            try
            {
                await _permissionsByRoleCache.RemoveAsync(key);
                _logger.LogInformation("Permisos de rol eliminados con clave: {Key}.", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar permisos de rol con clave: {Key}.", key);
                throw;
            }
        }
    }
}