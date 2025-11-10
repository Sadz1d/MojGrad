using MediatR;

namespace Market.Application.Modules.Civic.CitizenProposals.Queries.GetById;

public sealed class GetCitizenProposalByIdQuery : IRequest<GetCitizenProposalByIdQueryDto>
{
    public int Id { get; init; }
}
