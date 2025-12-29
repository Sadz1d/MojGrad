using FluentValidation;

namespace Market.Application.Modules.Surveys.Commands.Status.Disable;

public sealed class DisableSurveyCommandValidator
    : AbstractValidator<DisableSurveyCommand>
{
    public DisableSurveyCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
