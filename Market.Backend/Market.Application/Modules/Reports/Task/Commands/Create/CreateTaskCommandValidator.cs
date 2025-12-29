using FluentValidation;
using Market.Application.Modules.Reports.Tasks.Commands.Create;
using Market.Domain.Entities.Reports;

namespace Market.Application.Modules.Tasks.Commands.Create;

public sealed class CreateTaskCommandValidator
    : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.ReportId)
            .GreaterThan(0);

        RuleFor(x => x.WorkerId)
            .GreaterThan(0);

        RuleFor(x => x.TaskStatus)
            .NotEmpty()
            .MaximumLength(TaskEntity.Constraints.StatusMaxLength);

        RuleFor(x => x.Deadline)
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.Deadline.HasValue)
            .WithMessage("Deadline must be in the future.");
    }
}
