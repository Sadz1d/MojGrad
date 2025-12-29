using MediatR;

namespace Market.Application.Modules.Civic.CitizenProposals.Commands.Status.Enable;

public sealed class EnableCitizenProposalCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}
