using Microsoft.AspNetCore.Mvc;
using AccessAppUser.Infrastructure.Repositories.Interfaces;
using AccessAppUser.Infrastructure.DTOs;

namespace AccessAppUser.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<T> : ControllerBase where T : class
    {
        protected readonly IBaseRepository<T> _repository;

        public BaseController(IBaseRepository<T> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<GenericResponseDTO<IEnumerable<T>>>> GetAll()
        {
            var result = await _repository.GetAllAsync();
            return Ok(GenericResponseDTO<IEnumerable<T>>.SuccessResponse(result, 200, "Datos obtenidos correctamente"));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResponseDTO<T>>> GetById(Guid id)
        {
            var result = await _repository.GetByIdAsync(id);
            if (result == null)
                return NotFound(GenericResponseDTO<T>.ErrorResponse(404, "Elemento no encontrado"));

            return Ok(GenericResponseDTO<T>.SuccessResponse(result, 200, "Elemento encontrado"));
        }

        [HttpPost]
        public async Task<ActionResult<GenericResponseDTO<T>>> Create(T entity)
        {
            await _repository.AddAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity }, GenericResponseDTO<T>.SuccessResponse(entity, 201, "Elemento creado correctamente"));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GenericResponseDTO<T>>> Update(Guid id, T entity)
        {
            if (!await _repository.ExistsAsync(id))
                return NotFound(GenericResponseDTO<T>.ErrorResponse(404, "Elemento no encontrado"));

            await _repository.UpdateAsync(entity);
            return Ok(GenericResponseDTO<T>.SuccessResponse(entity, 200, "Elemento actualizado correctamente"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GenericResponseDTO<T>>> Delete(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (!await _repository.ExistsAsync(id))
                return NotFound(GenericResponseDTO<T>.ErrorResponse(404, "Elemento no encontrado"));

            await _repository.DeleteAsync(id);
            return Ok(GenericResponseDTO<T>.SuccessResponse(entity !, 200, "Elemento eliminado correctamente"));
        }
    }
}
