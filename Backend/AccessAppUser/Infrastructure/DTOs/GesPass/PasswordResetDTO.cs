namespace AccessAppUser.Infrastructure.DTOs.GesPass
{
    public class PasswordResetDTO
    {
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
