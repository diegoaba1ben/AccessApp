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
            Console.WriteLine($"Usuarios obtenidos de la BD: {users?.Count()}");

            // Verifica si hay usuarios en la base de datos
            if (users == null || !users.Any())
            {
                Console.WriteLine("No hay usuarios registradoes en BD");
                return NotFound("No hay usuarios registrados");
            }

            var mappedUsers = _mapper.Map<IEnumerable<UserReadDTO>>(users);
            Console.WriteLine($" Usuarios mapeados por AutoMapper: {mappedUsers?.Count()}");

            // Verifica si el mapeo de AutoMapper falló
            if (mappedUsers == null || !mappedUsers.Any())
            {
                Console.WriteLine("Error al mapear los usuarios");
                return NotFound("Error al mapear los usuarios");
            }

            Console.WriteLine("Devuelve la lista de usuarios correctamente");
            return Ok(mappedUsers);
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
            Console.WriteLine($" Intento de login: Email={userDto.Email}, Password={userDto.Password}");

            var allUsers = await _userRepository.GetAllAsync();
            Console.WriteLine(" Usuarios en la base de datos:");
            foreach (var u in allUsers)
            {
                Console.WriteLine($"- {u.Email} | {u.Password}");
            }

            var user = await _userRepository.GetByEmailAsync(userDto.Email);

            if (user == null)
            {
                Console.WriteLine(" Usuario no encontrado en la BD.");
                return Unauthorized("Credenciales incorrectas");
            }

            Console.WriteLine($" Usuario encontrado: {user.Email}, Password almacenada: {user.Password}");

            if (user.Password != userDto.Password) // Comparación directa 
            {
                Console.WriteLine($" Contraseña incorrecta: ingresada={userDto.Password}, almacenada={user.Password}");
                return Unauthorized("Credenciales incorrectas");
            }

            Console.WriteLine(" Inicio de sesión exitoso");
            return Ok("Inicio de sesión exitoso");
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