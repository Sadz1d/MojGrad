using MediatR;

namespace Market.Application.Modules.Civic.CitizenProposals.Commands.Status.Disable;

public sealed class DisableCitizenProposalCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}
