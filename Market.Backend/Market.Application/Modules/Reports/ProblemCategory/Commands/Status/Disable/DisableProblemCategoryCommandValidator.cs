using FluentValidation;

namespace Market.Application.Modules.Reports.ProblemCategories.Commands.Status.Disable;

public sealed class DisableProblemCategoryCommandValidator
    : AbstractValidator<DisableProblemCategoryCommand>
{
    public DisableProblemCategoryCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
