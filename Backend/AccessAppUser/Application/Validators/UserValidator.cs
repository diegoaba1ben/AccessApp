using FluentValidation;
using AccessAppUser.Domain.Entities;
using System.Text.RegularExpressions;

namespace AccessAppUser.Application.Validators
{
    /// <summary>
    /// Entidad encargada de validar los datos de un usuario.
    /// </summary>
    public class UserValidator : AbstractValidator<User>
    {
        // Definir mensajes de error reutilizables
        private const string NameRequiredMsg = "El nombre del usuario es requerido.";
        private const string NameLengthMsg = "El nombre debe tener entre 3 y 50 caracteres.";
        private const string NameFormatMsg = "El nombre solo puede contener letras y espacios.";

        private const string EmailRequiredMsg = "El correo electrónico es requerido.";
        private const string EmailFormatMsg = "El correo electrónico no tiene un formato válido.";
        private const string EmailLengthMsg = "El correo electrónico debe tener entre 5 y 50 caracteres.";

        private const string PasswordRequiredMsg = "La contraseña es requerida.";
        private const string PasswordLengthMsg = "La contraseña debe tener entre 8 y 20 caracteres.";
        private const string PasswordFormatMsg = "La contraseña debe contener al menos una mayúscula, una minúscula, un número y un carácter especial.";

        private const string RolesRequiredMsg = "El usuario debe tener al menos un rol asignado.";
        private const string RolesRequiredForInitializedMsg = "El usuario debe tener al menos un rol asignado después de la inicialización del sistema.";

        public UserValidator()
        {
            // Validación del Nombre
            RuleFor(user => user.Name)
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

                    if (!Regex.IsMatch(name, @"^[\p{L}\s]+$"))  // Permite caracteres de todos los idiomas
                    {
                        context.AddFailure(NameFormatMsg);
                    }
                });

            // Validación del Correo Electrónico
            RuleFor(user => user.Email)
                .Custom((email, context) =>
                {
                    if (string.IsNullOrWhiteSpace(email))
                    {
                        context.AddFailure(EmailRequiredMsg);
                        return;
                    }

                    email = email.Trim();

                    if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) // Validación de formato de email
                    {
                        context.AddFailure(EmailFormatMsg);
                    }

                    if (email.Length < 5 || email.Length > 50)
                    {
                        context.AddFailure(EmailLengthMsg);
                    }
                });

            // Validación de la Contraseña
            RuleFor(user => user.Password)
                .Custom((password, context) =>
                {
                    if (string.IsNullOrWhiteSpace(password))
                    {
                        context.AddFailure(PasswordRequiredMsg);
                        return;
                    }

                    if (password.Length < 8 || password.Length > 20)
                    {
                        context.AddFailure(PasswordLengthMsg);
                    }

                    if (!Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,20}$"))
                    {
                        context.AddFailure(PasswordFormatMsg);
                    }
                });

            // Validación de Roles
            RuleFor(user => user.Roles)
                .Must((user, roles) => roles != null && (roles.Count > 0 || !User.IsSystemInitialized))
                .WithMessage(user => User.IsSystemInitialized ? RolesRequiredForInitializedMsg : RolesRequiredMsg);
        }
    }
}