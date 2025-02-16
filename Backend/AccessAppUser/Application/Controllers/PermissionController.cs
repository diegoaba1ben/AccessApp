using Microsoft.AspNetCore.Mvc;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Repositories.Interfaces;

namespace AccessAppUser.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionController : BaseController<Permission>
    {
        public PermissionController(IPermissionRepository repository) : base(repository){}
    }
}