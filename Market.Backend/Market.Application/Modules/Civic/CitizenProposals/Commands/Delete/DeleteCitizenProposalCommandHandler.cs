using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Civic;

namespace Market.Application.Modules.Civic.CitizenProposals.Commands.Delete;

public sealed class DeleteCitizenProposalCommandHandler
    : IRequestHandler<DeleteCitizenProposalCommand, Unit>
{
    private readonly IAppDbContext _ctx;

    public DeleteCitizenProposalCommandHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<Unit> Handle(DeleteCitizenProposalCommand request, CancellationToken ct)
    {
        var proposal = await _ctx.CitizenProposals
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (proposal == null)
            throw new MarketNotFoundException($"Citizen proposal with Id {request.Id} not found.");

        _ctx.CitizenProposals.Remove(proposal);
        await _ctx.SaveChangesAsync(ct);

        return Unit.Value;
    }
}