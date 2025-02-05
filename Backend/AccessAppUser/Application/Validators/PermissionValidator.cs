using FluentValidation;
using AccessAppUser.Domain.Entities;
using System.Text.RegularExpressions;

namespace AccessAppUser.Application.Validators
{
    /// <summary>
    /// Validador para la entidad Permission, asegurando que los datos cumplan con los requisitos.
    /// </summary>
    public class PermissionValidator : AbstractValidator<Permission>
    {
        // Definir mensajes de error reutilizables
        private const string NameRequiredMsg = "El nombre del permiso es requerido.";
        private const string NameLengthMsg = "El nombre del permiso debe tener entre 3 y 50 caracteres.";
        private const string NameFormatMsg = "El nombre solo puede contener letras y espacios.";

        private const string DescriptionRequiredMsg = "La descripción del permiso es requerida.";
        private const string DescriptionLengthMsg = "La descripción del permiso debe tener entre 3 y 100 caracteres.";

        public PermissionValidator()
        {
            RuleFor(permission => permission.Name)
                .Custom((name, context) =>
                {
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        context.AddFailure(NameRequiredMsg);
                        return;
                    }

                    name = name.Trim();

                    if (name.Length < 3 || name.Length > 50)
                    {
                        context.AddFailure(NameLengthMsg);
                    }

                    if (!Regex.IsMatch(name, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
                    {
                        context.AddFailure(NameFormatMsg);
                    }
                });

            RuleFor(permission => permission.Description)
                .Custom((description, context) =>
                {
                    if (string.IsNullOrWhiteSpace(description))
                    {
                        context.AddFailure(DescriptionRequiredMsg);
                        return;
                    }

                    description = description.Trim();

                    if (description.Length < 3 || description.Length > 100)
                    {
                        context.AddFailure(DescriptionLengthMsg);
                    }
                });
        }
    }
}
