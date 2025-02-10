using Xunit;
using FluentValidation.TestHelper;
using AccessAppUser.Application.Validators;
using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Tests.Application.Validators
{
    public class PermissionValidatorTests
    {
        private readonly PermissionValidator _validator = new PermissionValidator();

        [Fact]
        public void Permission_Should_Have_Error_When_Name_Is_Empty()
        {
            // Arrange
            var permission = Permission.Builder()
                .WithName("")
                .WithDescription("Descripción válida")
                .Build();

            // Act
            var result = _validator.TestValidate(permission);

            // Assert
            result.ShouldHaveValidationErrorFor(p => p.Name)
                .WithErrorMessage("El nombre del permiso es requerido.");
        }

        [Fact]
        public void Permission_Should_Have_Error_When_Name_Is_Too_Short()
        {
            // Arrange
            var permission = Permission.Builder()
                .WithName("AB")
                .WithDescription("Descripción válida")
                .Build();

            // Act
            var result = _validator.TestValidate(permission);

            // Assert
            result.ShouldHaveValidationErrorFor(p => p.Name)
                .WithErrorMessage("El nombre del permiso debe tener entre 3 y 50 caracteres.");
        }

        [Fact]
        public void Permission_Should_Have_Error_When_Name_Contains_Invalid_Characters()
        {
            // Arrange
            var permission = Permission.Builder()
                .WithName("Permiso123!")
                .WithDescription("Descripción válida")
                .Build();

            // Act
            var result = _validator.TestValidate(permission);

            // Assert
            result.ShouldHaveValidationErrorFor(p => p.Name)
                .WithErrorMessage("El nombre solo puede contener letras y espacios.");
        }

        [Fact]
        public void Permission_Should_Have_Error_When_Description_Is_Empty()
        {
            // Arrange
            var permission = Permission.Builder()
                .WithName("Lectura")
                .WithDescription("")
                .Build();

            // Act
            var result = _validator.TestValidate(permission);

            // Assert
            result.ShouldHaveValidationErrorFor(p => p.Description)
                .WithErrorMessage("La descripción del permiso es requerida.");
        }

        [Fact]
        public void Permission_Should_Have_Error_When_Description_Is_Too_Short()
        {
            // Arrange
            var permission = Permission.Builder()
                .WithName("Lectura")
                .WithDescription("AB")
                .Build();

            // Act
            var result = _validator.TestValidate(permission);

            // Assert
            result.ShouldHaveValidationErrorFor(p => p.Description)
                .WithErrorMessage("La descripción del permiso debe tener entre 3 y 100 caracteres.");
        }

        [Fact]
        public void Permission_Should_Be_Valid_With_Correct_Values()
        {
            // Arrange
            var permission = Permission.Builder()
                .WithName("Editar contenido")
                .WithDescription("Permiso que permite modificar contenido en la plataforma")
                .Build();

            // Act
            var result = _validator.TestValidate(permission);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
