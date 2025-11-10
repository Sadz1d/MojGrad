using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Civic;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Civic.CitizenProposals.Queries.GetById;

public sealed class GetCitizenProposalByIdQueryHandler
    : IRequestHandler<GetCitizenProposalByIdQuery, GetCitizenProposalByIdQueryDto>
{
    private readonly IAppDbContext _ctx;
    public GetCitizenProposalByIdQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<GetCitizenProposalByIdQueryDto> Handle(
        GetCitizenProposalByIdQuery request, CancellationToken ct)
    {
        var proposal = await _ctx.CitizenProposals
            .AsNoTracking()
            .Include(p => p.User)
            .Where(p => p.Id == request.Id)
            .Select(p => new GetCitizenProposalByIdQueryDto
            {
                Id = p.Id,
                Title = p.Title,
                Text = p.Text,
                PublicationDate = p.PublicationDate,
                IsEnabled = p.IsEnabled,
                AuthorName = p.User != null
                    ? (p.User.FirstName + " " + p.User.LastName).Trim()
                    : "Anonimno"
            })
            .FirstOrDefaultAsync(ct);

        if (proposal == null)
            throw new MarketNotFoundException($"CitizenProposal with Id {request.Id} not found.");

        return proposal;
    }
}
