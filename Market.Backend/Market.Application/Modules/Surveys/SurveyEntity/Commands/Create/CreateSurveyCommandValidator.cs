using FluentValidation;
using Market.Application.Modules.Surveys.Survey.Commands.Create;
using Market.Domain.Entities.Surveys;

namespace Market.Application.Modules.Surveys.Commands.Create;

public sealed class CreateSurveyCommandValidator
    : AbstractValidator<CreateSurveyCommand>
{
    public CreateSurveyCommandValidator()
    {
        RuleFor(x => x.Question)
            .NotEmpty().WithMessage("Question is required.")
            .MaximumLength(SurveyEntity.Constraints.QuestionMaxLength)
            .WithMessage($"Question can be at most {SurveyEntity.Constraints.QuestionMaxLength} characters long.");

        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate)
            .WithMessage("Start date must be before end date.");
    }
}
