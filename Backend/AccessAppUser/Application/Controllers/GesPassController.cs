using Microsoft.AspNetCore.Mvc;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.Repositories.Interfaces;
using AccessAppUser.Infrastructure.DTOs.GesPass;
using System;
using System.Threading.Tasks;

namespace AccessAppUser.Application.Controllers
{
    [ApiController]
    [Route("api/gespass")]
    public class GesPassController : ControllerBase
    {
        private readonly IGesPassRepository _gesPassRepository;
        private readonly IUserRepository _userRepository;

        public GesPassController(IGesPassRepository gesPassRepository, IUserRepository userRepository)
        {
            _gesPassRepository = gesPassRepository;
            _userRepository = userRepository;
        }

        [HttpPost("request-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] PasswordResetRequestDTO request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
                return NotFound("Usuario no encontrado");

            var token = Guid.NewGuid().ToString(); // 🔥 Se mejorará con `AccessAppSecurity`
            var gesPass = new GesPass
            {
                UserId = user.Id,
                ResetToken = token,
                TokenExpiration = DateTime.UtcNow.AddHours(1)
            };

            await _gesPassRepository.AddAsync(gesPass);
            return Ok($"Token generado: {token}"); // 🔥 En el futuro, se enviará por email
        }

        [HttpPost("validate-token")]
        public async Task<IActionResult> ValidateResetToken([FromBody] TokenValidationDTO request)
        {
            var gesPass = await _gesPassRepository.GetByResetTokenAsync(request.Token);
            if (gesPass == null || gesPass.TokenExpiration < DateTime.UtcNow)
                return BadRequest("Token inválido o expirado");

            return Ok("Token válido");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetDTO request)
        {
            var gesPass = await _gesPassRepository.GetByResetTokenAsync(request.Token);
            if (gesPass == null || gesPass.TokenExpiration < DateTime.UtcNow)
                return BadRequest("Token inválido o expirado");

            var user = await _userRepository.GetByIdAsync(gesPass.UserId);
            if (user == null)
                return NotFound("Usuario no encontrado");

            user.Password = request.NewPassword; // 🔥 Se mejorará con hashing
            await _userRepository.UpdateAsync(user);
            await _gesPassRepository.DeleteAsync(gesPass.Id); // Eliminar token usado

            return Ok("Contraseña restablecida correctamente");
        }
    }
}
