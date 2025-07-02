using FluentValidation.TestHelper;
using TrainComponentManagement.BLL.DTOs;
using TrainComponentManagement.BLL.Validation;

namespace TrainComponentManagement.Tests.Validators
{
    public class CreateComponentDtoValidatorTests
    {
        private readonly CreateComponentDtoValidator _validator = new();

        [Fact]
        public void Should_HaveError_When_NameIsNullOrTooLong()
        {
            // Arrange
            var dto = new CreateOrUpdateComponentDto { Name = null!, UniqueNumber = "X", CanAssignQuantity = false };

            // Act & Assert
            _validator.TestValidate(dto)
                      .ShouldHaveValidationErrorFor(x => x.Name);

            // Arrange
            dto.Name = new string('A', 201);

            // Act & Assert
            _validator.TestValidate(dto)
                      .ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_HaveError_When_UniqueNumberIsNullOrTooLong()
        {
            // Arrange
            var dto = new CreateOrUpdateComponentDto { Name = "N", UniqueNumber = null!, CanAssignQuantity = false };

            // Act & Assert
            _validator.TestValidate(dto)
                      .ShouldHaveValidationErrorFor(x => x.UniqueNumber);

            // Arrange
            dto.UniqueNumber = new string('X', 51);

            // Act & Assert
            _validator.TestValidate(dto)
                      .ShouldHaveValidationErrorFor(x => x.UniqueNumber);
        }

        [Fact]
        public void Should_HaveError_When_QuantityMissingOrNegative_And_CanAssignQuantityTrue()
        {
            // Arrange
            var dto = new CreateOrUpdateComponentDto
            {
                Name = "A",
                UniqueNumber = "U",
                CanAssignQuantity = true,
                Quantity = null
            };

            // Act & Assert
            _validator.TestValidate(dto)
                      .ShouldHaveValidationErrorFor(x => x.Quantity);

            // Arrange
            dto.Quantity = -1;

            // Act & Assert
            _validator.TestValidate(dto)
                      .ShouldHaveValidationErrorFor(x => x.Quantity);
        }

        [Fact]
        public void Should_HaveError_When_QuantityNotNull_And_CanAssignQuantityFalse()
        {
            // Arrange
            var dto = new CreateOrUpdateComponentDto
            {
                Name = "A",
                UniqueNumber = "U",
                CanAssignQuantity = false,
                Quantity = 5
            };

            // Act & Assert
            _validator.TestValidate(dto)
                      .ShouldHaveValidationErrorFor(x => x.Quantity);
        }
    }
}
