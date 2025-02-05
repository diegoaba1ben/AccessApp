using FluentValidation;
using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Application.Validators
{
    /// <summary>
    /// Valida las relaciones de un área con otras entidades.
    /// </summary>
    public class AreaRelationshipValidator : AbstractValidator<Area>
    {
        public AreaRelationshipValidator()
        {
            RuleFor(area => area.Roles)
                .NotNull().WithMessage("El área debe estar asociada al menos a un rol.")
                .Must(roles => roles.Count > 0).WithMessage("El área debe contener al menos un rol.");

            RuleFor(area => area.AreaProfiles)
                .NotNull().WithMessage("El área debe estar asociada a al menos un perfil.")
                .Must(profiles => profiles.Count > 0).WithMessage("El área debe estar relacionada con al menos un perfil.");
        }
    }
}
