using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using AccessAppUser.Infrastructure.Repositories.Interfaces;
using AccessAppUser.Infrastructure.DTOs;
using AccessAppUser.Infrastructure.Cache.Interfaces;

namespace AccessAppUser.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<T> : ControllerBase where T : class
    {
        protected readonly IBaseRepository<T> _repository;
        protected readonly IBaseCacheService<T> _cacheService;
        private readonly string _cacheKeyPrefix;

        public BaseController(IBaseRepository<T> repository, IBaseCacheService<T> cacheService)
        {
            _repository = repository;
            _cacheService = cacheService;
            _cacheKeyPrefix = typeof(T).Name;
        }

        [HttpGet]
        public async Task<ActionResult<GenericResponseDTO<IEnumerable<T>>>> GetAll()
        {
            var cacheKey = $"{_cacheKeyPrefix}-all";
            var cachedData = await _cacheService.GetAsync(cacheKey);

            if (cachedData != null)
            {
                // Deserializar cachedData a IEnumerable<T>
                var cachedList = JsonSerializer.Deserialize<IEnumerable<T>>(JsonSerializer.Serialize(cachedData));
                if (cachedList != null)
                {
                    return Ok(GenericResponseDTO<IEnumerable<T>>.SuccessResponse(cachedList, 200, "Datos obtenidos de la caché"));
                }                
            }

            var result = await _repository.GetAllAsync();
            if (result.Any())
            {
                await _cacheService.SetAsync<IEnumerable<T>>(cacheKey, result.ToList(), TimeSpan.FromMinutes(5)); // Caché por 5 minutos
            }
            return Ok(GenericResponseDTO<IEnumerable<T>>.SuccessResponse(result, 200, "Datos obtenidos correctamente"));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResponseDTO<T>>> GetById(Guid id)
        {
            var cacheKey = $"{_cacheKeyPrefix}-{id}";
            var cachedData = await _cacheService.GetAsync(cacheKey);

            if (cachedData != null)
            {
                return Ok(GenericResponseDTO<T>.SuccessResponse(cachedData, 200, "Elemento obtenido de la caché"));
            }

            var result = await _repository.GetByIdAsync(id);
            if (result == null)
                return NotFound(GenericResponseDTO<T>.ErrorResponse(404, "Elemento no encontrado"));

            await _cacheService.SetAsync(cacheKey, new List<T> {result}, TimeSpan.FromMinutes(5)); // Caché por 5 minutos
            return Ok(GenericResponseDTO<T>.SuccessResponse(result, 200, "Elemento encontrado"));
        }

        [HttpPost]
        public async Task<ActionResult<GenericResponseDTO<T>>> Create(T entity)
        {
            await _repository.AddAsync(entity);
            // Invalida la caché de GetAll y el elemento individual
            await _cacheService.RemoveAsync($"{_cacheKeyPrefix}-all");
            var id = typeof(T).GetProperty("Id")?.GetValue(entity);
            if (id != null)
            {
                await _cacheService.RemoveAsync($"{_cacheKeyPrefix}-{id}");
            }

            return CreatedAtAction(nameof(GetById), new { id = entity }, GenericResponseDTO<T>.SuccessResponse(entity, 201, "Elemento creado correctamente"));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GenericResponseDTO<T>>> Update(Guid id, T entity)
        {
            if (!await _repository.ExistsAsync(id))
                return NotFound(GenericResponseDTO<T>.ErrorResponse(404, "Elemento no encontrado"));

            await _repository.UpdateAsync(entity);
            // Invalida la caché de GetAll y el elemento individual
            await _cacheService.RemoveAsync($"{_cacheKeyPrefix}-all");
            await _cacheService.RemoveAsync($"{_cacheKeyPrefix}-{id}");
            return Ok(GenericResponseDTO<T>.SuccessResponse(entity, 200, "Elemento actualizado correctamente"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GenericResponseDTO<T>>> Delete(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (!await _repository.ExistsAsync(id))
                return NotFound(GenericResponseDTO<T>.ErrorResponse(404, "Elemento no encontrado"));

            await _repository.DeleteAsync(id);
            // Invalida la caché de GetAll y el elemento individual
            await _cacheService.RemoveAsync($"{_cacheKeyPrefix}-all");
            await _cacheService.RemoveAsync($"{_cacheKeyPrefix}-{id}");
            return Ok(GenericResponseDTO<T>.SuccessResponse(entity!, 200, "Elemento eliminado correctamente"));
        }
    }
}