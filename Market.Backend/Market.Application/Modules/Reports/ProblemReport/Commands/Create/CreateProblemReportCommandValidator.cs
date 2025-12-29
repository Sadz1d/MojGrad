using FluentValidation;
using Market.Application.Modules.Reports.ProblemReport.Commands.Create;
using Market.Domain.Entities.Reports;

namespace Market.Application.Modules.Reports.ProblemReports.Commands.Create;

public sealed class CreateProblemReportCommandValidator
    : AbstractValidator<CreateProblemReportCommand>
{
    public CreateProblemReportCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(ProblemReportEntity.Constraints.TitleMaxLength)
            .WithMessage($"Title can be at most {ProblemReportEntity.Constraints.TitleMaxLength} characters long.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(ProblemReportEntity.Constraints.DescriptionMaxLength)
            .WithMessage($"Description can be at most {ProblemReportEntity.Constraints.DescriptionMaxLength} characters long.");

        RuleFor(x => x.Location)
            .MaximumLength(ProblemReportEntity.Constraints.LocationMaxLength)
            .When(x => !string.IsNullOrWhiteSpace(x.Location))
            .WithMessage($"Location can be at most {ProblemReportEntity.Constraints.LocationMaxLength} characters long.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0);

        RuleFor(x => x.StatusId)
            .GreaterThan(0);
    }
}
