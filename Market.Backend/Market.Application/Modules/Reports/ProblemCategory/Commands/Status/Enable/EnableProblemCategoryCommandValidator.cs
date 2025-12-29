using FluentValidation;

namespace Market.Application.Modules.Reports.ProblemCategories.Commands.Status.Enable;

public sealed class EnableProblemCategoryCommandValidator
    : AbstractValidator<EnableProblemCategoryCommand>
{
    public EnableProblemCategoryCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
