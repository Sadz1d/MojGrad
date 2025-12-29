using FluentValidation;
using Market.Application.Modules.Reports.ProblemCategory.Commands.Create;
using Market.Domain.Entities.Reports;

namespace Market.Application.Modules.Reports.ProblemCategories.Commands.Create;

public sealed class CreateProblemCategoryCommandValidator
    : AbstractValidator<CreateProblemCategoryCommand>
{
    public CreateProblemCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(ProblemCategoryEntity.Constraints.NameMaxLength)
            .WithMessage($"Name can be at most {ProblemCategoryEntity.Constraints.NameMaxLength} characters long.");

        RuleFor(x => x.Description)
            .MaximumLength(ProblemCategoryEntity.Constraints.DescriptionMaxLength)
            .WithMessage($"Description can be at most {ProblemCategoryEntity.Constraints.DescriptionMaxLength} characters long.");
    }
}
