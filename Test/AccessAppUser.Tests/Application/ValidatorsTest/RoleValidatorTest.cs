using Xunit;
using FluentValidation.TestHelper;
using AccessAppUser.Application.Validators;
using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Tests.Application.Validators
{
    public class RoleValidatorTests
    {
        private readonly RoleValidator _validator = new();

        [Fact]
        public void Role_Should_Not_Allow_Empty_Name()
        {
            // Arrange
            var role = Role.Builder()
                .SetName("")
                .SetDescription("Descripción válida.")
                .Build();

            // Act
            var result = _validator.TestValidate(role);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Name)
                .WithErrorMessage("El nombre del rol es requerido.");
        }

        [Fact]
        public void Role_Should_Not_Allow_Short_Name()
        {
            // Arrange
            var role = Role.Builder()
                .SetName("Ad")
                .SetDescription("Descripción válida.")
                .Build();

            // Act
            var result = _validator.TestValidate(role);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Name)
                .WithErrorMessage("El nombre del rol debe tener entre 3 y 50 caracteres.");
        }

        [Fact]
        public void Role_Should_Not_Allow_Long_Name()
        {
            // Arrange
            var role = Role.Builder()
                .SetName(new string('A', 51)) // 51 caracteres
                .SetDescription("Descripción válida.")
                .Build();

            // Act
            var result = _validator.TestValidate(role);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Name)
                .WithErrorMessage("El nombre del rol debe tener entre 3 y 50 caracteres.");
        }

        [Fact]
        public void Role_Should_Not_Allow_Empty_Description()
        {
            // Arrange
            var role = Role.Builder()
                .SetName("Administrador")
                .SetDescription("")
                .Build();

            // Act
            var result = _validator.TestValidate(role);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Description)
                .WithErrorMessage("La descripción del rol es requerida.");
        }

        [Fact]
        public void Role_Should_Not_Allow_Short_Description()
        {
            // Arrange
            var role = Role.Builder()
                .SetName("Administrador")
                .SetDescription("1234") // 4 caracteres
                .Build();

            // Act
            var result = _validator.TestValidate(role);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Description)
                .WithErrorMessage("La descripción debe tener entre 5 y 100 caracteres.");
        }

        [Fact]
        public void Role_Should_Not_Allow_Long_Description()
        {
            // Arrange
            var role = Role.Builder()
                .SetName("Administrador")
                .SetDescription(new string('D', 101)) // 101 caracteres
                .Build();

            // Act
            var result = _validator.TestValidate(role);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Description)
                .WithErrorMessage("La descripción debe tener entre 5 y 100 caracteres.");
        }

        [Fact]
        public void Role_Should_Be_Valid_When_Correct()
        {
            // Arrange
            var role = Role.Builder()
                .SetName("Administrador")
                .SetDescription("Rol con acceso total al sistema.")
                .Build();

            // Act
            var result = _validator.TestValidate(role);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
