using FluentValidation;
using Market.Application.Modules.Surveys.Survey.Commands.Update;
using Market.Domain.Entities.Surveys;

namespace Market.Application.Modules.Surveys.Commands.Update;

public sealed class UpdateSurveyCommandValidator
    : AbstractValidator<UpdateSurveyCommand>
{
    public UpdateSurveyCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Question)
            .NotEmpty().WithMessage("Question is required.")
            .MaximumLength(SurveyEntity.Constraints.QuestionMaxLength)
            .WithMessage($"Question can be at most {SurveyEntity.Constraints.QuestionMaxLength} characters long.");

        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate)
            .WithMessage("Start date must be before end date.");
    }
}
