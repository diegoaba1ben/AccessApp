using Microsoft.AspNetCore.Mvc;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Repositories.Interfaces;

namespace AccessAppUser.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : BaseController<Profile>
    {
        public ProfileController(IProfileRepository repository) : base(repository) { }
    }
}
