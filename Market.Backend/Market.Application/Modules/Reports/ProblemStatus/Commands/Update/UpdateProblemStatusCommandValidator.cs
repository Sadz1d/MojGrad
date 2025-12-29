using FluentValidation;
using Market.Application.Modules.Reports.ProblemStatus.Commands.Update;
using Market.Domain.Entities.Reports;

namespace Market.Application.Modules.Reports.ProblemStatuses.Commands.Update;

public sealed class UpdateProblemStatusCommandValidator
    : AbstractValidator<UpdateProblemStatusCommand>
{
    public UpdateProblemStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(ProblemStatusEntity.Constraints.NameMaxLength)
            .WithMessage($"Name can be at most {ProblemStatusEntity.Constraints.NameMaxLength} characters long.");
    }
}
