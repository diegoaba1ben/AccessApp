using FluentValidation;
using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Application.Validators
{
    /// <summary>
    /// Valida la relación de un rol con las áreas asociadas.
    /// </summary>
    public class RoleRelationshipValidator : AbstractValidator<Role>
    {
        public RoleRelationshipValidator()
        {
            RuleFor(role => role.Areas)
                .NotNull().WithMessage("El rol debe estar asociado al menos a un área.")
                .Must(areas => areas.Count > 0).WithMessage("El rol debe contener al menos un área.");
        }
    }
}

