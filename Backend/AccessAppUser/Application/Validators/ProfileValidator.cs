using FluentValidation;
using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Application.Validators
{
    /// <summary>
    /// Validador para la entidad Profile, asegurando que los datos cumplan con los requisitos.
    /// </summary>
    public class ProfileValidator : AbstractValidator<Profile>
    {
        public ProfileValidator()
        {
            // Validación del ID
            RuleFor(profile => profile.Id)
                .NotEmpty().WithMessage("El ID del perfil es obligatorio.");

            // Validación de la fecha de creación
            RuleFor(profile => profile.CreatedAt.Date)
                .NotEmpty().WithMessage("La fecha de creación es obligatoria.")
                .LessThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("Verifique la fecha de creación.");

            // Validación de la relación con User
            RuleFor(profile => profile.User)
                .NotNull().WithMessage("Cada perfil debe estar asociado a un usuario.");

            // Validación de la relación con Role
            RuleFor(profile => profile.Role)
                .NotNull().WithMessage("Cada perfil debe tener un rol asignado.");

            // Validación de la lista de AreaProfiles
            RuleFor(profile => profile.AreaProfiles)
                .NotNull().WithMessage("El perfil debe tener al menos un área asociada.")
                .Must(areaProfiles => areaProfiles.Count > 0)
                .WithMessage("El perfil debe estar vinculado a al menos un área.");
        }
    }
}