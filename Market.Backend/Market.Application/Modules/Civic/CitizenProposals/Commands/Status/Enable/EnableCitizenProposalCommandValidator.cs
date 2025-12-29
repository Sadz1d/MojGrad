using FluentValidation;

namespace Market.Application.Modules.Civic.CitizenProposals.Commands.Status.Enable;

public sealed class EnableCitizenProposalCommandValidator
    : AbstractValidator<EnableCitizenProposalCommand>
{
    public EnableCitizenProposalCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
