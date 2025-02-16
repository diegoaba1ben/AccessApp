using Microsoft.AspNetCore.Mvc;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Repositories.Interfaces;
using AccessAppUser.Infrastructure.DTOs.User;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AccessAppUser.Application.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadDTO>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<UserReadDTO>>(users));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadDTO>> GetUserById(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound("Usuario no encontrado");

            return Ok(_mapper.Map<UserReadDTO>(user));
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserReadDTO>> RegisterUser(UserCreateDTO userDto)
        {
            var user = _mapper.Map<User>(userDto);
            await _userRepository.AddAsync(user);

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, _mapper.Map<UserReadDTO>(user));
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUser(UserLoginDTO userDto)
        {
            var user = await _userRepository.GetByEmailAsync(userDto.Email);
            if (user == null || user.Password != userDto.Password) // 🔥 Se debe mejorar con hashing
                return Unauthorized("Credenciales incorrectas");

            return Ok("Inicio de sesión exitoso"); // 🔥 Luego se debe cambiar por JWT
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, UserUpdateDTO userDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound("Usuario no encontrado");

            _mapper.Map(userDto, user);
            await _userRepository.UpdateAsync(user);

            return Ok("Usuario actualizado correctamente");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound("Usuario no encontrado");

            await _userRepository.DeleteAsync(id);
            return Ok("Usuario eliminado correctamente");
        }
    }
}
