using MediatR;

namespace Market.Application.Modules.Civic.CitizenProposals.Commands.Delete;

public sealed class DeleteCitizenProposalCommand : IRequest<Unit>
{
    public required int Id { get; init; }
}