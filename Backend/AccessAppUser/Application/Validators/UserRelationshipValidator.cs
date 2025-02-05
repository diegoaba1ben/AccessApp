using FluentValidation;
using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Application.Validators
{
    /// <summary>
    /// Valida la relación de un usuario con los roles asociados.
    /// </summary>
    public class UserRelationshipValidator : AbstractValidator<User>
    {
        public UserRelationshipValidator()
        {
            RuleFor(user => user.Roles)
                .NotNull().WithMessage("El usuario debe estar asociado al menos a un rol.")
                .Must(roles => roles.Count > 0).WithMessage("El usuario debe tener al menos un rol asignado.");
        }
    }
}
