using FluentValidation;
using Market.Application.Modules.Reports.Tasks.Commands.Update;
using Market.Domain.Entities.Reports;

namespace Market.Application.Modules.Tasks.Commands.Update;

public sealed class UpdateTaskCommandValidator
    : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskCommandValidator()
    {
        RuleFor(x => x.Id)
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
