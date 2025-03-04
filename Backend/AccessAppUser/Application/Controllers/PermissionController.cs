using Microsoft.AspNetCore.Mvc;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Repositories.Interfaces;
using AccessAppUser.Infrastructure.Cache.Interfaces;

namespace AccessAppUser.Application.Controllers
{
    /// <summary>
    /// Controlador de permisos
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionController : BaseController<Permission>
    {
// Campo privado de la clase
    private readonly IPermissionCacheService _permissionCacheService;

        // Constructor de la clase
        public PermissionController(IPermissionRepository repository, IPermissionCacheService permissionCacheService) : base(repository)
        {
            _permissionCacheService = permissionCacheService;
        }
    }
}