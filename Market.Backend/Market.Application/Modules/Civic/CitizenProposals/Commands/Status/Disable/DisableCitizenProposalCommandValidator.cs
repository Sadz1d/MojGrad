using FluentValidation;

namespace Market.Application.Modules.Civic.CitizenProposals.Commands.Status.Disable;

public sealed class DisableCitizenProposalCommandValidator
    : AbstractValidator<DisableCitizenProposalCommand>
{
    public DisableCitizenProposalCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
