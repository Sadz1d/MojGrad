using FluentValidation;
using Market.Application.Modules.Reports.ProblemReport.Commands.Update;
using Market.Domain.Entities.Reports;

namespace Market.Application.Modules.Reports.ProblemReports.Commands.Update;

public sealed class UpdateProblemReportCommandValidator
    : AbstractValidator<UpdateProblemReportCommand>
{
    public UpdateProblemReportCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(ProblemReportEntity.Constraints.TitleMaxLength);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(ProblemReportEntity.Constraints.DescriptionMaxLength);

        RuleFor(x => x.Location)
            .MaximumLength(ProblemReportEntity.Constraints.LocationMaxLength)
            .When(x => !string.IsNullOrWhiteSpace(x.Location));

        RuleFor(x => x.CategoryId)
            .GreaterThan(0);

        RuleFor(x => x.StatusId)
            .GreaterThan(0);
    }
}
