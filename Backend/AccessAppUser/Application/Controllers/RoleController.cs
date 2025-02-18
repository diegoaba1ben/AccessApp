using Microsoft.AspNetCore.Mvc;
using AccessAppUser.Infrastructure.Repositories.Interfaces;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.DTOs;
using AccessAppUser.Infrastructure.DTOs.Role;

namespace AccessAppUser.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : BaseController<Role>
    {
        private readonly IRoleRepository _roleRepository;

        public RoleController(IRoleRepository repository) : base(repository)
        {
            _roleRepository = repository;
        }

        // Obtener los permisos asociados a un rol
        [HttpGet("{id}/permissions")]
        public async Task<ActionResult<GenericResponseDTO<IEnumerable<Permission>>>> GetPermissions(Guid id)
        {
            var permissions = await _roleRepository.GetPermissionsByRoleIdAsync(id);
            return Ok(GenericResponseDTO<IEnumerable<Permission>>.SuccessResponse(permissions));
        }

        // Asignar un permiso a un rol
        [HttpPost("{id}/permissions/{permissionId}")]
        public async Task<ActionResult<GenericResponseDTO<string>>> AddPermissionToRole(Guid id, Guid permissionId)
        {
            var success = await _roleRepository.AssignPermissionToRoleAsync(id, permissionId);
            if (!success)
                return NotFound(GenericResponseDTO<string>.ErrorResponse(404, "Rol o Permiso no encontrado"));

            return Ok(GenericResponseDTO<string>.SuccessResponse("Permiso asignado al rol exitosamente"));
        }

        // Eliminar un permiso de un rol
        [HttpDelete("{id}/permissions/{permissionId}")]
        public async Task<ActionResult<GenericResponseDTO<string>>> RemovePermissionFromRole(Guid id, Guid permissionId)
        {
            var success = await _roleRepository.RemovePermissionFromRoleAsync(id, permissionId);
            if (!success)
                return NotFound(GenericResponseDTO<string>.ErrorResponse(404, "Rol o Permiso no encontrado"));

            return Ok(GenericResponseDTO<string>.SuccessResponse("Permiso eliminado del rol exitosamente"));
        }

        // Obtener los usuarios asociados a un rol
        [HttpGet("{id}/users")]
        public async Task<ActionResult<GenericResponseDTO<IEnumerable<User>>>> GetUsers(Guid id)
        {
            var users = await _roleRepository.GetUsersByRoleIdAsync(id);
            return Ok(GenericResponseDTO<IEnumerable<User>>.SuccessResponse(users));
        }

        // Obtener las áreas asociadas a un rol
        [HttpGet("{id}/areas")]
        public async Task<ActionResult<GenericResponseDTO<IEnumerable<Area>>>> GetAreas(Guid id)
        {
            var areas = await _roleRepository.GetAreasByRoleIdAsync(id);
            return Ok(GenericResponseDTO<IEnumerable<Area>>.SuccessResponse(areas));
        }
    }
}

