namespace AccessAppUser.Infrastructure.DTOs.User
{
    /// <summary>
    /// DTO para el inicio de sesión del usuario.
    /// </summary>
    public class UserLoginDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
