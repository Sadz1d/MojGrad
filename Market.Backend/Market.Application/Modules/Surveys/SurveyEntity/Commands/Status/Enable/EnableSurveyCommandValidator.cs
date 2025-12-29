using FluentValidation;

namespace Market.Application.Modules.Surveys.Commands.Status.Enable;

public sealed class EnableSurveyCommandValidator
    : AbstractValidator<EnableSurveyCommand>
{
    public EnableSurveyCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
