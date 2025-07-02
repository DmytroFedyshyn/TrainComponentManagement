using FluentValidation.TestHelper;
using TrainComponentManagement.BLL.DTOs;
using TrainComponentManagement.BLL.Validation;

namespace TrainComponentManagement.Tests.Validators
{
    public class ComponentDtoValidatorTests
    {
        private readonly ComponentDtoValidator _validator = new();

        [Fact]
        public void Should_HaveError_When_IdIsNotPositive()
        {
            // Arrange
            var dto = new ComponentDto
            {
                Id = 0,
                Name = "A",
                UniqueNumber = "U",
                CanAssignQuantity = false
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Fact]
        public void Should_Inherit_CreateRules()
        {
            // Arrange
            var dto = new ComponentDto
            {
                Id = 1,
                Name = new string('A', 201),
                UniqueNumber = new string('X', 51),
                CanAssignQuantity = true,
                Quantity = -5
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
            result.ShouldHaveValidationErrorFor(x => x.UniqueNumber);
            result.ShouldHaveValidationErrorFor(x => x.Quantity);
        }
    }
}
