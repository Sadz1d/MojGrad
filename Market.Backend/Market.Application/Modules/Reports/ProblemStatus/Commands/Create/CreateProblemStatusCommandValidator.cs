using FluentValidation;
using Market.Application.Modules.Reports.ProblemStatus.Commands.Create;
using Market.Domain.Entities.Reports;

namespace Market.Application.Modules.Reports.ProblemStatuses.Commands.Create;

public sealed class CreateProblemStatusCommandValidator
    : AbstractValidator<CreateProblemStatusCommand>
{
    public CreateProblemStatusCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(ProblemStatusEntity.Constraints.NameMaxLength)
            .WithMessage($"Name can be at most {ProblemStatusEntity.Constraints.NameMaxLength} characters long.");
    }
}
