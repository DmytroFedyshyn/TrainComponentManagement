using FluentValidation;
using TrainComponentManagement.BLL.DTOs;

namespace TrainComponentManagement.BLL.Validation
{
    public class CreateComponentDtoValidator : AbstractValidator<CreateOrUpdateComponentDto>
    {
        public CreateComponentDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");

            RuleFor(x => x.UniqueNumber)
                .NotEmpty().WithMessage("Unique Number is required.")
                .MaximumLength(50).WithMessage("Unique Number cannot exceed 50 characters.");

            When(x => x.CanAssignQuantity, () =>
            {
                RuleFor(x => x.Quantity)
                    .NotNull().WithMessage("Quantity must be specified when CanAssignQuantity is true.")
                    .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative.");
            });

            When(x => !x.CanAssignQuantity, () =>
            {
                RuleFor(x => x.Quantity)
                    .Null().WithMessage("Quantity must be null when CanAssignQuantity is false.");
            });
        }
    }
}
