using FluentValidation;
using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Application.Validators
{
    public class RoleValidator : AbstractValidator<Role>
    {
        public RoleValidator()
        {
            RuleFor(role => role.Name)
                .NotEmpty().WithMessage("El nombre del rol es requerido.")
                .Length(3, 50).WithMessage("El nombre del rol debe tener entre 3 y 50 caracteres.");

            RuleFor(role => role.Description)
                .NotEmpty().WithMessage("La descripción del rol es requerida.")
                .Length(5, 100).WithMessage("La descripción debe tener entre 5 y 100 caracteres.");

            // Validaciones de Relaciones
            //RuleFor(role => role).SetValidator(new RoleRelationshipValidator());
        }
    }
}