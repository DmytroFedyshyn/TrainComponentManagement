using FluentValidation;
using TrainComponentManagement.BLL.DTOs;

namespace TrainComponentManagement.BLL.Validation
{
    public class ComponentDtoValidator : AbstractValidator<ComponentDto>
    {
        public ComponentDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be a positive integer.");

            Include(new CreateComponentDtoValidator());
        }
    }
}
