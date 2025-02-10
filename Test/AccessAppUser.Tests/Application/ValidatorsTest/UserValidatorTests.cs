using Xunit;
using FluentAssertions;
using FluentValidation.TestHelper;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Application.Validators;
using System;

namespace AccessAppUser.Tests.Application.Builders
{
    public class UserValidatorTests
    {
        private readonly UserValidator _validator = new UserValidator();

        [Fact]
        public void User_Should_Have_Valid_Name()
        {
            // Arrange
            var user = User.Builder()
                .WithName("Carlos Pérez")
                .WithEmail("carlos.perez@email.com")
                .WithPassword("Pass@1234")
                .Build();

            // Act
            var result = _validator.TestValidate(user);

            // Assert
            result.ShouldNotHaveValidationErrorFor(u => u.Name);
        }

        [Fact]
        public void User_Should_Not_Allow_Empty_Name()
        {
            // Arrange
            var user = User.Builder()
                .WithName("")
                .WithEmail("test@email.com")
                .WithPassword("SecurePass1!")
                .Build();

            // Act
            var result = _validator.TestValidate(user);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.Name)
                .WithErrorMessage("El nombre del usuario es requerido.");
        }

        [Fact]
        public void User_Should_Not_Allow_Short_Name()
        {
            // Arrange
            var user = User.Builder()
                .WithName("A")  // Demasiado corto
                .WithEmail("test@email.com")
                .WithPassword("SecurePass1!")
                .Build();

            // Act
            var result = _validator.TestValidate(user);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.Name)
                .WithErrorMessage("El nombre debe tener entre 3 y 50 caracteres.");
        }

        [Fact]
        public void User_Should_Have_Valid_Email()
        {
            // Arrange
            var user = User.Builder()
                .WithName("Maria López")
                .WithEmail("maria.lopez@email.com")
                .WithPassword("SecurePass1!")
                .Build();

            // Act
            var result = _validator.TestValidate(user);

            // Assert
            result.ShouldNotHaveValidationErrorFor(u => u.Email);
        }

        [Fact]
        public void User_Should_Not_Allow_Invalid_Email()
        {
            // Arrange
            var user = User.Builder()
                .WithName("Mario Torres")
                .WithEmail("mario.email.com")  // Formato inválido
                .WithPassword("SecurePass1!")
                .Build();

            // Act
            var result = _validator.TestValidate(user);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.Email)
                .WithErrorMessage("El correo electrónico no tiene un formato válido.");
        }

        [Fact]
        public void User_Should_Have_Valid_Password()
        {
            // Arrange
            var user = User.Builder()
                .WithName("Camila Rodríguez")
                .WithEmail("camila.rodriguez@email.com")
                .WithPassword("Strong@2024")
                .Build();

            // Act
            var result = _validator.TestValidate(user);

            // Assert
            result.ShouldNotHaveValidationErrorFor(u => u.Password);
        }

        [Fact]
        public void User_Should_Not_Allow_Weak_Password()
        {
            // Arrange
            var user = User.Builder()
                .WithName("Andrés Mejía")
                .WithEmail("andres.mejia@email.com")
                .WithPassword("12345678")  // Débil
                .Build();

            // Act
            var result = _validator.TestValidate(user);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.Password)
                .WithErrorMessage("La contraseña debe contener al menos una mayúscula, una minúscula, un número y un carácter especial.");
        }
    }
}
