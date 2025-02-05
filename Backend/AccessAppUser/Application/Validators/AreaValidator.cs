using FluentValidation;
using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Application.Validators
{
    /// <summary>
    /// Valida los datos de un área funcional dentro del sistema.
    /// </summary>
    public class AreaValidator : AbstractValidator<Area>
    {
        public AreaValidator()
        {
            RuleFor(area => area.Name)
                .NotEmpty().WithMessage("El nombre del área es requerido.")
                .Length(3, 50).WithMessage("El nombre del área debe tener entre 3 y 50 caracteres.")
                .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$")
                .WithMessage("El nombre del área solo puede contener letras y espacios.");

            RuleFor(area => area.Description)
                .NotEmpty().WithMessage("La descripción del área es requerida.")
                .Length(10, 100).WithMessage("La descripción del área debe tener entre 10 y 100 caracteres.");
        }
    }
}
