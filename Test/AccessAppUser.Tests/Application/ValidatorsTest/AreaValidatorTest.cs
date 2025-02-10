using Xunit;
using FluentValidation.TestHelper;
using AccessAppUser.Application.Validators;
using AccessAppUser.Domain.Entities;

namespace AccessAppUser.Tests.Application.Validators
{
    public class AreaValidatorTests
    {
        private readonly AreaValidator _validator = new();

        [Fact]
        public void Area_Should_Be_Valid_When_Correct()
        {
            var area = Area.Builder()
                .SetName("Finanzas")
                .SetDescription("Área encargada de gestionar los recursos financieros")
                .Build();

            var result = _validator.TestValidate(area);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Area_Should_Not_Allow_Empty_Name()
        {
            var area = Area.Builder()
                .SetName("")
                .SetDescription("Área válida")
                .Build();

            var result = _validator.TestValidate(area);

            result.ShouldHaveValidationErrorFor(a => a.Name)
                .WithErrorMessage("El nombre del área es requerido.");
        }

        [Fact]
        public void Area_Should_Not_Allow_Short_Name()
        {
            var area = Area.Builder()
                .SetName("AB")
                .SetDescription("Área válida")
                .Build();

            var result = _validator.TestValidate(area);

            result.ShouldHaveValidationErrorFor(a => a.Name)
                .WithErrorMessage("El nombre del área debe tener entre 3 y 50 caracteres.");
        }

        [Fact]
        public void Area_Should_Not_Allow_Long_Name()
        {
            var area = Area.Builder()
                .SetName(new string('A', 51)) // Más de 50 caracteres
                .SetDescription("Área válida")
                .Build();

            var result = _validator.TestValidate(area);

            result.ShouldHaveValidationErrorFor(a => a.Name)
                .WithErrorMessage("El nombre del área debe tener entre 3 y 50 caracteres.");
        }

        [Fact]
        public void Area_Should_Not_Allow_Invalid_Name_Format()
        {
            var area = Area.Builder()
                .SetName("Área 123!")
                .SetDescription("Área válida")
                .Build();

            var result = _validator.TestValidate(area);

            result.ShouldHaveValidationErrorFor(a => a.Name)
                .WithErrorMessage("El nombre del área solo puede contener letras y espacios.");
        }

        [Fact]
        public void Area_Should_Not_Allow_Empty_Description()
        {
            var area = Area.Builder()
                .SetName("Finanzas")
                .SetDescription("")
                .Build();

            var result = _validator.TestValidate(area);

            result.ShouldHaveValidationErrorFor(a => a.Description)
                .WithErrorMessage("La descripción del área es requerida.");
        }

        [Fact]
        public void Area_Should_Not_Allow_Short_Description()
        {
            var area = Area.Builder()
                .SetName("Finanzas")
                .SetDescription("Corta")
                .Build();

            var result = _validator.TestValidate(area);

            result.ShouldHaveValidationErrorFor(a => a.Description)
                .WithErrorMessage("La descripción del área debe tener entre 10 y 100 caracteres.");
        }

        [Fact]
        public void Area_Should_Not_Allow_Long_Description()
        {
            var area = Area.Builder()
                .SetName("Finanzas")
                .SetDescription(new string('D', 101)) // Más de 100 caracteres
                .Build();

            var result = _validator.TestValidate(area);

            result.ShouldHaveValidationErrorFor(a => a.Description)
                .WithErrorMessage("La descripción del área debe tener entre 10 y 100 caracteres.");
        }
    }
}
